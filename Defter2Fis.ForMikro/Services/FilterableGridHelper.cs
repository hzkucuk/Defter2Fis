using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// KryptonDataGridView'lere filtre (arama) ve kolon sıralama yeteneği ekleyen yardımcı sınıf.
    /// Her grid için ayrı bir instance oluşturulur.
    /// </summary>
    internal sealed class FilterableGridHelper<T> : IDisposable where T : class
    {
        private readonly KryptonDataGridView _dgv;
        private readonly KryptonTextBox _txtFiltre;
        private List<T> _tumVeri;
        private List<T> _filtrelenmisVeri;
        private BindingSource _bindingSource;
        private string _siralamaKolonu;
        private ListSortDirection _siralamaYonu = ListSortDirection.Ascending;
        private PropertyInfo[] _ozellikler;
        private Action<DataGridViewRowCollection> _satirRenklendirme;
        private bool _yukleniyor;

        /// <summary>
        /// Yeni bir filtrelenebilir grid yardımcısı oluşturur.
        /// </summary>
        /// <param name="dgv">Hedef grid</param>
        /// <param name="txtFiltre">Arama kutusu</param>
        public FilterableGridHelper(KryptonDataGridView dgv, KryptonTextBox txtFiltre)
        {
            _dgv = dgv ?? throw new ArgumentNullException(nameof(dgv));
            _txtFiltre = txtFiltre ?? throw new ArgumentNullException(nameof(txtFiltre));

            _txtFiltre.TextChanged += TxtFiltre_TextChanged;
            _dgv.ColumnHeaderMouseClick += Dgv_ColumnHeaderMouseClick;
        }

        /// <summary>
        /// Veriyi grid'e yükler. Filtre ve sıralama sıfırlanır.
        /// </summary>
        /// <param name="satirRenklendirme">Opsiyonel: her veri yükleme/filtre sonrası satır renklendirme callback'i.</param>
        public void VeriYukle(List<T> veri, Action<DataGridViewColumnCollection> kolonAyarla, Action<DataGridViewRowCollection> satirRenklendirme = null)
        {
            _yukleniyor = true;
            try
            {
                _satirRenklendirme = satirRenklendirme;
                _dgv.DataSource = null;
                _dgv.Columns.Clear();
                _txtFiltre.Text = string.Empty;
                _siralamaKolonu = null;
                _siralamaYonu = ListSortDirection.Ascending;

                if (veri == null || veri.Count == 0)
                {
                    _tumVeri = new List<T>();
                    _filtrelenmisVeri = new List<T>();
                    return;
                }

                _tumVeri = veri;
                _filtrelenmisVeri = new List<T>(veri);
                _ozellikler = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                _bindingSource = new BindingSource { DataSource = _filtrelenmisVeri };
                _dgv.DataSource = _bindingSource;

                if (_dgv.Columns.Count > 0)
                    kolonAyarla(_dgv.Columns);

                // Kolon başlıklarına sıralama desteği
                foreach (DataGridViewColumn col in _dgv.Columns)
                    col.SortMode = DataGridViewColumnSortMode.Programmatic;

                _satirRenklendirme?.Invoke(_dgv.Rows);
            }
            finally
            {
                _yukleniyor = false;
            }
        }

        /// <summary>Filtrelenmiş veri listesini döner.</summary>
        public List<T> FiltrelenmisVeri => _filtrelenmisVeri ?? new List<T>();

        private void TxtFiltre_TextChanged(object sender, EventArgs e)
        {
            if (_yukleniyor) return;
            FiltreUygula();
        }

        private void FiltreUygula()
        {
            if (_tumVeri == null) return;

            string aramaMetni = (_txtFiltre.Text ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(aramaMetni))
            {
                _filtrelenmisVeri = new List<T>(_tumVeri);
            }
            else
            {
                string aramaKucuk = aramaMetni.ToLowerInvariant();
                _filtrelenmisVeri = _tumVeri.Where(satir => SatirEslesiyor(satir, aramaKucuk)).ToList();
            }

            // Aktif sıralama varsa koru
            if (!string.IsNullOrEmpty(_siralamaKolonu))
                SiralamaUygula();

            GridYenile();
        }

        private bool SatirEslesiyor(T satir, string aramaKucuk)
        {
            foreach (var prop in _ozellikler)
            {
                object deger = prop.GetValue(satir, null);
                if (deger == null) continue;

                string metin;
                if (deger is DateTime dt)
                    metin = dt.ToString("dd.MM.yyyy");
                else if (deger is decimal dec)
                    metin = dec.ToString("N2");
                else
                    metin = deger.ToString();

                if (metin.ToLowerInvariant().Contains(aramaKucuk))
                    return true;
            }
            return false;
        }

        private void Dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.ColumnIndex >= _dgv.Columns.Count) return;
            if (_filtrelenmisVeri == null || _filtrelenmisVeri.Count == 0) return;

            string kolonAdi = _dgv.Columns[e.ColumnIndex].DataPropertyName;
            if (string.IsNullOrEmpty(kolonAdi))
                kolonAdi = _dgv.Columns[e.ColumnIndex].Name;

            // Aynı kolona tekrar tıklanınca yön değiştir
            if (_siralamaKolonu == kolonAdi)
            {
                _siralamaYonu = _siralamaYonu == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }
            else
            {
                _siralamaKolonu = kolonAdi;
                _siralamaYonu = ListSortDirection.Ascending;
            }

            SiralamaUygula();
            GridYenile();
            SiralamaGlifiGuncelle(e.ColumnIndex);
        }

        private void SiralamaUygula()
        {
            if (string.IsNullOrEmpty(_siralamaKolonu) || _filtrelenmisVeri == null) return;

            var prop = _ozellikler.FirstOrDefault(p =>
                p.Name.Equals(_siralamaKolonu, StringComparison.OrdinalIgnoreCase));

            if (prop == null) return;

            if (_siralamaYonu == ListSortDirection.Ascending)
                _filtrelenmisVeri = _filtrelenmisVeri.OrderBy(x => prop.GetValue(x, null)).ToList();
            else
                _filtrelenmisVeri = _filtrelenmisVeri.OrderByDescending(x => prop.GetValue(x, null)).ToList();
        }

        private void GridYenile()
        {
            if (_bindingSource != null)
            {
                _bindingSource.DataSource = _filtrelenmisVeri;
                _bindingSource.ResetBindings(false);
            }
            _satirRenklendirme?.Invoke(_dgv.Rows);
        }

        private void SiralamaGlifiGuncelle(int aktifKolonIndex)
        {
            foreach (DataGridViewColumn col in _dgv.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;

            if (aktifKolonIndex >= 0 && aktifKolonIndex < _dgv.Columns.Count)
            {
                _dgv.Columns[aktifKolonIndex].HeaderCell.SortGlyphDirection =
                    _siralamaYonu == ListSortDirection.Ascending
                        ? SortOrder.Ascending
                        : SortOrder.Descending;
            }
        }

        /// <summary>Kaynakları serbest bırakır.</summary>
        public void Dispose()
        {
            _txtFiltre.TextChanged -= TxtFiltre_TextChanged;
            _dgv.ColumnHeaderMouseClick -= Dgv_ColumnHeaderMouseClick;
        }
    }
}
