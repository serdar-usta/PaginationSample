using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pagination
{
    public partial class Form1 : Form
    {
        int _pageNumber = 1;
        int _pageSize;
        int _totalPageCount = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbPageSize.Items.Add(10);
            cmbPageSize.Items.Add(25);
            cmbPageSize.Items.Add(50);
            cmbPageSize.SelectedItem = _pageSize;
            //cmbPageSize.SelectedIndex = 0;

            _totalPageCount = GetTotalCount(_pageSize);

            //lblPageNumber.Text = string.Format("{0} / {1}", _pageNumber.ToString(), _totalPageCount);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_pageNumber + 1 <= _totalPageCount)
            {
                _pageNumber++;

                BindGridData(_pageSize, _pageNumber);

                lblPageNumber.Text = string.Format("{0} / {1}", _pageNumber.ToString(), _totalPageCount);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (_pageNumber - 1 > 0)
            {
                _pageNumber--;

                BindGridData(_pageSize, _pageNumber);

                lblPageNumber.Text = string.Format("{0} / {1}", _pageNumber.ToString(), _totalPageCount);
            }
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPageSize.SelectedIndex > -1)
            {
                _pageSize = (int)cmbPageSize.SelectedItem;
                _pageNumber = 1;
                _totalPageCount = GetTotalCount(_pageSize);

                BindGridData(_pageSize, _pageNumber);

                lblPageNumber.Text = string.Format("{0} / {1}", _pageNumber.ToString(), _totalPageCount);
            }
        }

        void BindGridData(int pageSize, int pageNumber)
        {
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;

            NorthwindContext context = new NorthwindContext();



            dataGridView1.DataSource = context
            .Products
            .Select(p => new { p.ProductID, p.ProductName, p.Category.CategoryName, p.UnitPrice })
            .OrderBy(p => p.ProductID)
            .Skip(skip)
            .Take(take)
            .ToList();
        }

        int GetTotalCount(int pageSize)
        {
            int totalProductsCount = 0;
            using (NorthwindContext context = new NorthwindContext())
            {
                totalProductsCount = context.Products.Count();
            }
            int totalPageCount = (int)Math.Ceiling((double)totalProductsCount / pageSize);
            return totalPageCount;
        }
    }
}
