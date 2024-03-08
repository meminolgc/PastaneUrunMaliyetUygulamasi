using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PastaneUrunMaliyetUygulamasi
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		SqlConnection baglanti = new SqlConnection(@"Data Source=emin\SQLEXPRESS;Initial Catalog=MaliyetDb;Integrated Security=True;Encrypt=False");

		void malzemeListe()
		{
			SqlDataAdapter da = new SqlDataAdapter("Select * From tblmalzemeler", baglanti);
			DataTable dt = new DataTable();
			da.Fill(dt);
			dataGridView1.DataSource = dt;
		}
		void urunListesi()
		{
			SqlDataAdapter da2 = new SqlDataAdapter("Select * from tblurunler", baglanti);
			DataTable dt2 = new DataTable();
			da2.Fill(dt2);
			dataGridView1.DataSource = dt2;
		}
		void kasa()
		{
			SqlDataAdapter da3 = new SqlDataAdapter("Select * from tblkasa", baglanti);
			DataTable dt3 = new DataTable();
			da3.Fill(dt3);
			dataGridView1.DataSource = dt3;
		}
		void urunler()
		{
			baglanti.Open();
			SqlDataAdapter da = new SqlDataAdapter("Select * from TblUrunler", baglanti);
			DataTable dt = new DataTable();
			da.Fill(dt);
			CmbUrun.ValueMember = "URUNID";
			CmbUrun.DisplayMember = "AD";
			CmbUrun.DataSource = dt;
			baglanti.Close();
		}
		void malzemeler()
		{
			baglanti.Open();
			SqlDataAdapter da = new SqlDataAdapter("Select * from tblmalzemeler", baglanti);
			DataTable dt = new DataTable();
			da.Fill(dt);
			CmbMalzeme.ValueMember = "MALZEMEID";
			CmbMalzeme.DisplayMember = "AD";
			CmbMalzeme.DataSource = dt;
			baglanti.Close();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			malzemeListe();
			urunler();
			malzemeler();
		}

		private void BtnMalzemeListesi_Click(object sender, EventArgs e)
		{
			malzemeListe();
		}

		private void BtnUrunListesi_Click(object sender, EventArgs e)
		{
			urunListesi();
		}

		private void BtnKasa_Click(object sender, EventArgs e)
		{
			kasa();
		}

		private void BtnMalzemeEkle_Click(object sender, EventArgs e)
		{
			baglanti.Open();
			SqlCommand komut = new SqlCommand("insert into tblmalzemeler (ad,stok,fıyat,notlar) values (@p1,@p2,@p3,@p4)", baglanti);
			komut.Parameters.AddWithValue("@p1", TxtMalzemeAd.Text);
			komut.Parameters.AddWithValue("@p2", decimal.Parse(TxtMalzemeStok.Text));
			komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtMalzemeFiyat.Text));
			komut.Parameters.AddWithValue("@p4", TxtMalzemeNot.Text);
			komut.ExecuteNonQuery();
			baglanti.Close();
			MessageBox.Show("Malzeme Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
			malzemeListe();
		}

		private void BtnUrunEkle_Click(object sender, EventArgs e)
		{
			baglanti.Open();
			SqlCommand komut = new SqlCommand("insert into tblurunler (ad) values (@p1)", baglanti);
			komut.Parameters.AddWithValue("@p1", TxtUrunAd.Text);
			komut.ExecuteNonQuery();
			baglanti.Close();
			MessageBox.Show("Ürün Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
			urunListesi();
		}

		private void BtnUrunOlustur_Click(object sender, EventArgs e)
		{
			baglanti.Open();
			SqlCommand komut = new SqlCommand("insert into tblfırın (urunıd, malzemeıd, mıktar, malıyet) values (@p1,@p2,@p3,@p4)", baglanti);
			komut.Parameters.AddWithValue("@p1", CmbUrun.SelectedValue);
			komut.Parameters.AddWithValue("@p2", CmbMalzeme.SelectedValue);
			komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtMiktar.Text));
			komut.Parameters.AddWithValue("@p4", decimal.Parse(TxtMaliyet.Text));
			komut.ExecuteNonQuery();
			baglanti.Close();
			MessageBox.Show("Malzeme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

			listBox1.Items.Add(CmbMalzeme.Text + " - " + TxtMaliyet.Text);
		}

		private void TxtMiktar_TextChanged(object sender, EventArgs e)
		{
			double maliyet;
			if (TxtMiktar.Text == "")
			{
				TxtMiktar.Text = "0";
			}
			baglanti.Open();
			SqlCommand komut = new SqlCommand("Select * from tblmalzemeler where Malzemeıd=@p1", baglanti);
			komut.Parameters.AddWithValue("@p1", CmbMalzeme.SelectedValue);
			SqlDataReader dr = komut.ExecuteReader();
			while (dr.Read())
			{
				TxtMaliyet.Text = dr[3].ToString();
			}
			baglanti.Close();

			maliyet = Convert.ToDouble(TxtMaliyet.Text) / 1000 * Convert.ToDouble(TxtMiktar.Text);

			TxtMaliyet.Text = maliyet.ToString();
		}

		private void CmbMalzeme_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int secilen = dataGridView1.SelectedCells[0].RowIndex;

			TxtUrunID.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
			TxtUrunAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();

			baglanti.Open();
			SqlCommand komut = new SqlCommand("Select sum(Malıyet) from tblfırın where urunıd=@p1", baglanti);
			komut.Parameters.AddWithValue("@p1", TxtUrunID.Text);
			SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
			{
				TxtUrunMFıyat.Text = dr[0].ToString();
			}
			baglanti.Close();
        }

		private void BtnCikis_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
