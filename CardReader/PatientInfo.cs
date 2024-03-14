using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSIT155_E_MID
{
    public partial class PatientInfo : UserControl
    {

        private EntitySmartCardInfoType _info;
        public EntitySmartCardInfoType info
        {
            get { return _info; }
            set
            {
                if (_info == null)
                    _info = new EntitySmartCardInfoType();
                _info = value;
                txtName = _info?.cName;
                txtGender = _info?.cGender;
                txtID = _info?.cID;
                txtBirth = _info?.cBirthday;
            }
        }
        public void Clear(PatientInfo x)
        {
            x.txtName = "";
            x.txtGender = "";
            x.txtID = "";
            x.txtBirth = "";
        }

        public string txtName
        {
            get { return txtCardName.Text; }
            set { txtCardName.Text = value; }
        }
        public string txtGender
        {
            get { return txtCardGender.Text; }
            set { txtCardGender.Text = value; }
        }
        public string txtID
        {
            get { return txtCardId.Text; }
            set { txtCardId.Text = value; }
        }
        public string txtBirth
        {
            get { return txtCardBirth.Text; }
            set { txtCardBirth.Text = value; }
        }
        public PatientInfo()
        {
            InitializeComponent();
        }
    }

    public class EntitySmartCardInfoType
    {
        public EntitySmartCardInfoType() { }
        public EntitySmartCardInfoType(EntitySmartCardInfoType x)
        {
            cCardNumber = x.cCardNumber;
            cName = x.cName;
            cID = x.cID;
            cBirthday = x.cBirthday;
            cGender = x.cGender;
            cCardPublish = x.cCardPublish;
        }
        public string cCardNumber { get; set; }
        public string cName { get; set; }
        public string cID { get; set; }
        public string cBirthday { get; set; }
        public string cGender { get; set; }
        public string cCardPublish { get; set; }
    }
}
