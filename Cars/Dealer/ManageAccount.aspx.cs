using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using CarsBL.Transactions;
using System.Data;
using CarsInfo;
using CarsBL.Dealer;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public partial class _Default : System.Web.UI.Page
{
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet dsDropDown = new DataSet();
    UsedCarsInfo objUsedCarsInfo = new UsedCarsInfo();
    UserRegistrationInfo objUserInfo = new UserRegistrationInfo();


    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session[Constants.NAME] == null)
        {
            Response.Redirect("../login.aspx");
        }
        else
        {

            if (!IsPostBack)
            {
                int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());

                dsDropDown = objdropdownBL.Usp_Get_DropDown();

                FillStates();


                DealerActions objdealeract = new DealerActions();

                DataSet dsUserInfoDetails = objdealeract.DealerDefaultSellerInfo(Session[Constants.DealerCode].ToString(), UID);

                //DataSet dsUserInfoDetails = objdropdownBL.USP_GetUSerDetailsByUserID(UID);

                Session["getRegUserdata"] = dsUserInfoDetails;

                lblRegName.Text = dsUserInfoDetails.Tables[0].Rows[0]["sellerName"].ToString();
                //lblUserNameData.Text = dsUserInfoDetails.Tables[0].Rows[0]["Name"].ToString();
                //address1,    address2,    city,    state,    zip,    country,    phone,    altPhone,    fax,    ,    altEmail,    notesForBuyers,    IsDefault,    

                hdnsellarID.Value = dsUserInfoDetails.Tables[0].Rows[0]["sellerID"].ToString();

                DataSet dsDelaerLogo = objdealeract.GetDealerLogo(hdnsellarID.Value);

                if (dsDelaerLogo.Tables[0].Rows.Count > 0)
                {
                    imgthumb.ImageUrl = dsDelaerLogo.Tables[0].Rows[0]["LogoPath"].ToString();

                }

                //lblUserAddPack.Text = dsUserInfoDetails.Tables[0].Rows[0]["Name"].ToString();
                //lblUnamePW.Text = dsUserInfoDetails.Tables[0].Rows[0]["UserID"].ToString();

                lblRegEmail2.Text = dsUserInfoDetails.Tables[0].Rows[0]["email"].ToString();
                lblRegEmail.Text = dsUserInfoDetails.Tables[0].Rows[0]["email"].ToString();
                //lblEmailReg.Text = dsUserInfoDetails.Tables[0].Rows[0]["UserName"].ToString();
                lblRegPhone.Text = objGeneralFunc.filPhnm(dsUserInfoDetails.Tables[0].Rows[0]["Phone"].ToString());


                //lblBusinessName.Text = dsUserInfoDetails.Tables[0].Rows[0]["BusinessName"].ToString();
                lblAltEmail.Text = dsUserInfoDetails.Tables[0].Rows[0]["AltEmail"].ToString();
                lblAltPhone.Text = objGeneralFunc.filPhnm(dsUserInfoDetails.Tables[0].Rows[0]["AltPhone"].ToString());

                ArrayList strAddress = new ArrayList();
                strAddress.Add(dsUserInfoDetails.Tables[0].Rows[0]["Address1"].ToString().Trim());
                strAddress.Add(dsUserInfoDetails.Tables[0].Rows[0]["City"].ToString().Trim());
                strAddress.Add(GetStateName(dsUserInfoDetails.Tables[0].Rows[0]["State"].ToString()));
                strAddress.Add(dsUserInfoDetails.Tables[0].Rows[0]["Zip"].ToString());
                if (strAddress[0].ToString() != "" && (strAddress[1].ToString() != "" || strAddress[2].ToString() != "UN" || strAddress[3].ToString() != ""))
                {

                    lblRegAddress.Text = GeneralFunc.ToProper(strAddress[0].ToString()).Trim() + ", ";
                }
                else
                {
                    lblRegAddress.Text = GeneralFunc.ToProper(strAddress[0].ToString()).Trim();
                }

                if (strAddress[1].ToString() != "" && (strAddress[2].ToString() != "UN" || strAddress[3].ToString() != ""))
                {
                    lblRegCity.Text = GeneralFunc.ToProper(strAddress[1].ToString()).Trim() + ", ";
                }
                else
                {
                    lblRegCity.Text = GeneralFunc.ToProper(strAddress[1].ToString()).Trim();
                }

                if ((strAddress[2].ToString() != "UN") && (strAddress[3].ToString() != ""))
                {
                    lblRegState.Text = strAddress[2].ToString() + " ";
                }
                else if ((strAddress[2].ToString() != "UN") && (strAddress[3].ToString() == ""))
                {
                    lblRegState.Text = strAddress[2].ToString();
                }
                lblRegZip.Text = strAddress[3].ToString();

                FillUsers();
            }
        }
    }

    private string GetStateName(string StateID)
    {
        string sState = string.Empty;
        try
        {
            DropdownBL objdropdownBL = new DropdownBL();
            DataSet dsDropDown = new DataSet();

            if (System.Web.HttpContext.Current.Session["DsDropDown"] == null)
            {
                dsDropDown = objdropdownBL.Usp_Get_DropDown();
                System.Web.HttpContext.Current.Session["DsDropDown"] = dsDropDown;
            }
            else
            {
                dsDropDown = (DataSet)System.Web.HttpContext.Current.Session["DsDropDown"];
            }
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            dv = dsDropDown.Tables[1].DefaultView;
            dv.RowFilter = "State_ID='" + StateID + "'";

            dt = dv.ToTable();

            if (dt.Rows.Count > 0)
            {
                sState = dt.Rows[0]["State_Code"].ToString();
            }



            //ddlLocationState.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
        return sState;
    }

    private void FillUsers()
    {
        DealerUsers objUsers = new DealerUsers();
        DataSet dsUsers = new DataSet();
        try
        {
            dsUsers = objUsers.GetDealerGetUsers(Session[Constants.DealerCode].ToString());

            if (dsUsers.Tables[0].Rows.Count > 0)
            {
                grdUsers.DataSource = dsUsers.Tables[0];
                grdUsers.DataBind();
                Session["DealerUsers"] = dsUsers.Tables[0];

                DataView dv = new DataView();
                DataTable dt = new DataTable();
                dv = dsUsers.Tables[0].DefaultView;
                dv.RowFilter = "isdefault=1";
                dt = dv.ToTable();

                ArrayList strAddress = new ArrayList();
                strAddress.Add(dt.Rows[0]["Address"].ToString().Trim());
                strAddress.Add(dt.Rows[0]["City"].ToString().Trim());
                strAddress.Add(dt.Rows[0]["State_Code"].ToString());
                strAddress.Add(dt.Rows[0]["Zip"].ToString());

                lblDealerBName.Text = dt.Rows[0]["BusinessName"].ToString().Trim();
                lblDealerCode.Text = dt.Rows[0]["DealerCode"].ToString().Trim();


                if (strAddress[0].ToString() != "0" && (strAddress[1].ToString() != "" || strAddress[2].ToString() != "UN" || strAddress[3].ToString() != ""))
                {
                    lblDealerAddress.Text = GeneralFunc.ToProper(strAddress[0].ToString()).Trim() + ", ";
                }
                else
                {
                    lblDealerAddress.Text = GeneralFunc.ToProper(strAddress[0].ToString()).Trim();
                }

                if (strAddress[1].ToString() != "" && (strAddress[2].ToString() != "UN" || strAddress[3].ToString() != ""))
                {
                    lblDealerCity.Text = GeneralFunc.ToProper(strAddress[1].ToString()).Trim() + ", ";
                }
                else
                {
                    lblDealerCity.Text = GeneralFunc.ToProper(strAddress[1].ToString()).Trim();
                }

                if ((strAddress[2].ToString() != "UN") && (strAddress[3].ToString() != ""))
                {
                    lblDealerState.Text = strAddress[2].ToString() + " ";
                }
                else if ((strAddress[2].ToString() != "UN") && (strAddress[3].ToString() == ""))
                {
                    lblDealerState.Text = strAddress[2].ToString();
                }
                lblDealerZip.Text = strAddress[3].ToString();
                lblDealerPhone.Text = objGeneralFunc.filPhnm(dt.Rows[0]["Phonenumber"].ToString().Trim());
                lblDealerEmail.Text = dt.Rows[0]["UserName"].ToString().Trim();

            }




        }
        catch (Exception ex)
        {

        }


    }
    private void FillStates()
    {
        try
        {
            ddlLocationState.DataSource = dsDropDown.Tables[1];
            ddlLocationState.DataTextField = "State_Code";
            ddlLocationState.DataValueField = "State_ID";
            ddlLocationState.DataBind();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnChangePW_Click(object sender, EventArgs e)
    {
        try
        {
            string NewPwd = txtNewPW.Text;
            int userID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            bool bnew = objdropdownBL.USP_CHANGE_PASSWORD(NewPwd, userID);
            if (bnew == true)
            {
                mpeChangePW.Hide();
                MpeAlert.Show();
                lblErrorMSg.Visible = true;
                lblErrorMSg.Text = "Password changed successfully.";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            tbl1LblsDisplay.Style["display"] = "none";
            tbl2textDisplay.Style["display"] = "table";

            DealerActions objdealeract = new DealerActions();

            DataSet dsCarSellerInfo = objdealeract.DealerDefaultSellerInfo(Session[Constants.DealerCode].ToString(), Convert.ToInt32(Session[Constants.USER_ID].ToString()));

            btnEditDetails.Visible = false;
            btnUpdateDetails.Visible = true;
            divlblRegName.Style["display"] = "none";
            divtxtRegName.Style["display"] = "block";

            txtregName.Text = GeneralFunc.ToProper(dsCarSellerInfo.Tables[0].Rows[0]["sellerName"].ToString()).Trim();

            txtAltEmail.Text = dsCarSellerInfo.Tables[0].Rows[0]["AltEmail"].ToString();
            txtAltPhone.Text = dsCarSellerInfo.Tables[0].Rows[0]["AltPhone"].ToString();

            lblRegEmail.Text = dsCarSellerInfo.Tables[0].Rows[0]["email"].ToString();

            divlblRegPhone.Style["display"] = "none";
            divtxtRegPhone.Style["display"] = "block";
            txtRegPhone.Text = dsCarSellerInfo.Tables[0].Rows[0]["Phone"].ToString();

            divlblRegCity.Style["display"] = "none";
            divtxtRegCity.Style["display"] = "block";
            txtRegCity.Text = GeneralFunc.ToProper(dsCarSellerInfo.Tables[0].Rows[0]["City"].ToString()).Trim();

            divlblRegState.Style["display"] = "none";
            divddlRegState.Style["display"] = "block";

            ddlLocationState.SelectedIndex = ddlLocationState.Items.IndexOf(ddlLocationState.Items.FindByValue(dsCarSellerInfo.Tables[0].Rows[0]["State"].ToString()));

            divlblRegZip.Style["display"] = "none";
            divtxtRegZip.Style["display"] = "block";
            txtRegZip.Text = dsCarSellerInfo.Tables[0].Rows[0]["Zip"].ToString();

            divlblRegAddress.Style["display"] = "none";
            divtxtRegAddress.Style["display"] = "none";
            txtRegAddress.Text = GeneralFunc.ToProper(dsCarSellerInfo.Tables[0].Rows[0]["Address1"].ToString()).Trim();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnUpdateDetails_Click(object sender, EventArgs e)
    {
        try
        {

            objUserInfo.Name = GeneralFunc.ToProper(txtregName.Text).Trim();
            objUserInfo.Address = GeneralFunc.ToProper(txtRegAddress.Text).Trim();
            objUserInfo.City = GeneralFunc.ToProper(txtRegCity.Text).Trim();
            objUserInfo.StateID = Convert.ToInt32(ddlLocationState.SelectedItem.Value);
            if (txtRegZip.Text.Length == 4)
            {
                objUserInfo.Zip = "0" + txtRegZip.Text;
            }
            else
            {
                objUserInfo.Zip = txtRegZip.Text;
            }
            objUserInfo.PhoneNumber = txtRegPhone.Text;

            objUserInfo.UId = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            objUserInfo.UserName = lblRegEmail.Text;
            //objUserInfo.BusinessName = txtBusinessName.Text;
            objUserInfo.AltEmail = txtAltEmail.Text;
            objUserInfo.AltPhone = txtAltPhone.Text;

            DealerActions objDealeraction = new DealerActions();

            DataSet dsCarDetailsInfo = new DataSet();

            dsCarDetailsInfo = objDealeraction.UpdateDealerSellerInfo(objUserInfo, hdnsellarID.Value, Session[Constants.DealerCode].ToString(), lblRegEmail.Text);

            tbl1LblsDisplay.Style["display"] = "block";
            tbl2textDisplay.Style["display"] = "none";
            Session["getRegUserdata"] = dsCarDetailsInfo;

            btnEditDetails.Visible = true;
            btnUpdateDetails.Visible = false;

            tbl1LblsDisplay.Style["display"] = "block";
            tbl2textDisplay.Style["display"] = "none";

            divlblRegName.Style["display"] = "block";
            divtxtRegName.Style["display"] = "none";
            lblRegName.Text = dsCarDetailsInfo.Tables[0].Rows[0]["sellerName"].ToString();

            divlblRegPhone.Style["display"] = "block";
            divtxtRegPhone.Style["display"] = "none";
            lblRegPhone.Text = objGeneralFunc.filPhnm(dsCarDetailsInfo.Tables[0].Rows[0]["Phone"].ToString());

            lblRegEmail2.Text = dsCarDetailsInfo.Tables[0].Rows[0]["email"].ToString();
            lblAltEmail.Text = dsCarDetailsInfo.Tables[0].Rows[0]["AltEmail"].ToString();

            lblAltPhone.Text = objGeneralFunc.filPhnm(dsCarDetailsInfo.Tables[0].Rows[0]["AltPhone"].ToString());


            divlblRegCity.Style["display"] = "block";
            divtxtRegCity.Style["display"] = "none";


            divlblRegState.Style["display"] = "block";
            divddlRegState.Style["display"] = "none";


            divlblRegZip.Style["display"] = "block";
            divtxtRegZip.Style["display"] = "none";


            divlblRegAddress.Style["display"] = "block";
            divtxtRegAddress.Style["display"] = "none";


            ArrayList strAddress = new ArrayList();

            strAddress.Add(dsCarDetailsInfo.Tables[0].Rows[0]["Address1"].ToString().Trim());
            strAddress.Add(dsCarDetailsInfo.Tables[0].Rows[0]["City"].ToString().Trim());
            strAddress.Add(dsCarDetailsInfo.Tables[0].Rows[0]["State_Code"].ToString());
            strAddress.Add(dsCarDetailsInfo.Tables[0].Rows[0]["Zip"].ToString());

            lblRegAddress.Text = "";
            lblRegCity.Text = "";
            lblRegState.Text = "";
            lblRegZip.Text = "";
            if (strAddress[0].ToString() != "" && (strAddress[1].ToString() != "" || strAddress[2].ToString() != "UN" || strAddress[3].ToString() != ""))
            {
                lblRegAddress.Text = GeneralFunc.ToProper(strAddress[0].ToString()).Trim() + ", ";
            }
            else
            {
                lblRegAddress.Text = GeneralFunc.ToProper(strAddress[0].ToString()).Trim();
            }

            if (strAddress[1].ToString() != "" && (strAddress[2].ToString() != "UN" || strAddress[3].ToString() != ""))
            {
                lblRegCity.Text = GeneralFunc.ToProper(strAddress[1].ToString()).Trim() + ", ";
            }
            else
            {
                lblRegCity.Text = GeneralFunc.ToProper(strAddress[1].ToString()).Trim();
            }

            if ((strAddress[2].ToString() != "UN") || (strAddress[3].ToString() != ""))
            {
                lblRegState.Text = strAddress[2].ToString() + " ";
            }
            else if ((strAddress[2].ToString() == "UN") || (strAddress[3].ToString() == ""))
            {
                lblRegState.Text = strAddress[2].ToString();
            }
            lblRegZip.Text = strAddress[3].ToString();




        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditRow")
        {
            DataTable dtUsers = new DataTable();
            dtUsers = (DataTable)Session["DealerUsers"];

            DataView dv = new DataView();
            DataTable dt = new DataTable();
            dv = dtUsers.DefaultView;
            dv.RowFilter = "UId=" + e.CommandArgument + "";

            dt = dv.ToTable();
            txtEditName.Text = dt.Rows[0]["Name"].ToString();
            txtEditUserID.Text = dt.Rows[0]["UserID"].ToString();
            txtEditUserName.Text = dt.Rows[0]["UserName"].ToString();
            txtEditPhoneNumber.Text = dt.Rows[0]["PhoneNumber"].ToString();
            txtEditUId.Value = dt.Rows[0]["Uid"].ToString();
            hdnOldPassword.Value = dt.Rows[0]["Pwd"].ToString();

            if (dt.Rows[0]["isActive"].ToString() == "True")
            {
                ddlEditActive.SelectedIndex = 0;
            }
            else
            {
                ddlEditActive.SelectedIndex = 1;
            }

            MpeEdituser.Show();

        }
    }
    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        DealerUsers objUsers = new DealerUsers();

        DataTable dtUsers = new DataTable();

        dtUsers = (DataTable)Session["DealerUsers"];

        DataView dv = new DataView();

        DataTable dt = new DataTable();

        dv = dtUsers.DefaultView;

        dv.RowFilter = "UserID='" + txtUserID.Text + "'";

        dt = dv.ToTable();

        if (dt.Rows.Count == 0)
        {
            objUsers.AddDealerUsers(Session[Constants.DealerCode].ToString(), "0", txtName.Text, txtUserID.Text, txtPassword.Text, txtUserName.Text, txtPhoneNumber.Text, Convert.ToInt32(ddlActive.SelectedItem.Value));

            FillUsers();
        }
        else
        {
            MpeAlert.Show();
            lblErrorMSg.Visible = true;
            lblErrorMSg.Text = "User ID already exists.";

        }


    }
    protected void btnEdit_Click1(object sender, EventArgs e)
    {
        DealerUsers objUsers = new DealerUsers();

        objUsers.AddDealerUsers(Session[Constants.DealerCode].ToString(), txtEditUId.Value, txtEditName.Text, txtEditUserID.Text, txtEditNewPassword.Text, txtEditUserName.Text, txtEditPhoneNumber.Text, Convert.ToInt32(ddlEditActive.SelectedItem.Value));

        FillUsers();

    }

    protected void btnAddUsers_Click(object sender, EventArgs e)
    {
        MpeAdduser.Show();
    }
    protected void btnSaveLogo_Click(object sender, EventArgs e)
    {
        string Filepath = "Dealer//DealerImages//";

        string FileNameFullPath = Server.MapPath(Filepath);

        if (System.IO.Directory.Exists(FileNameFullPath) == false)
        {
            System.IO.Directory.CreateDirectory(FileNameFullPath);
        }
        fileUP.SaveAs(FileNameFullPath + hdnsellarID.Value + ".jpg");

        Bitmap oBitmap = default(Bitmap);
        oBitmap = new Bitmap(FileNameFullPath + hdnsellarID.Value + ".jpg");
        Graphics oGraphic = default(Graphics);

        int newwidthimg = 105;
        // Here create a new bitmap object of the same height and width of the image.
        float AspectRatio = (float)oBitmap.Size.Width / (float)oBitmap.Size.Height;

        int newHeight = Convert.ToInt32(newwidthimg / AspectRatio);

        Bitmap bmpNew = new Bitmap(newwidthimg, newHeight);
        oGraphic = Graphics.FromImage(bmpNew);

        oGraphic.CompositingQuality = CompositingQuality.HighQuality;
        oGraphic.SmoothingMode = SmoothingMode.HighQuality;
        oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;


        oGraphic.DrawImage(oBitmap, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), 0, 0, oBitmap.Width, oBitmap.Height, GraphicsUnit.Pixel);
        // Release the lock on the image file. Of course,
        // image from the image file is existing in Graphics object
        oBitmap.Dispose();
        oBitmap = bmpNew;

        //SolidBrush oBrush = new SolidBrush(Color.Black);
        //Font ofont = new Font("Arial", 8);
        //oGraphic.DrawString("Some text to write", ofont, oBrush, 10, 10);
        //oGraphic.Dispose();
        //ofont.Dispose();
        //oBrush.Dispose();
        oBitmap.Save(FileNameFullPath + hdnsellarID.Value + ".jpg", ImageFormat.Jpeg);

        oBitmap.Dispose();


        DealerActions objDealerActions = new DealerActions();

        string ImageURL = "Dealer/DealerImages/" + hdnsellarID.Value + ".jpg";

        DataSet dsSellerInfor = objDealerActions.DealerLogoUpdate(ImageURL, hdnsellarID.Value);

        if (dsSellerInfor.Tables[0].Rows.Count > 0)
        {
            imgthumb.ImageUrl = dsSellerInfor.Tables[0].Rows[0]["LogoPath"].ToString();            
        }
        Response.Redirect("ManageAccount.aspx"); 

        //DataSet dsDelaerLogo = objdealeract.GetDealerLogo(hdnsellarID.Value);

        //if (dsDelaerLogo.Tables[0].Rows.Count > 0)
        //{
        //    imgthumb.ImageUrl = dsDelaerLogo.Tables[0].Rows[0]["LogoPath"].ToString();
        //}
    }
    protected void grdUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbActive = (Label)e.Row.FindControl("lblActive");

            if (lbActive.Text == "True")
            {
                lbActive.Text = "Active";
            }
            else
            {
                lbActive.Text = "Inactive";
            }
            HiddenField hdnisdefault = (HiddenField)e.Row.FindControl("hdnisdefault");
            LinkButton lblEdit = (LinkButton)e.Row.FindControl("lblEdit");

            if (hdnisdefault.Value == "True")
            {
                lblEdit.Visible = false;
                //hdnisdefault.Text = "Active";
            }
            else
            {
                lblEdit.Visible = true;
            }
            Label lblPhoneNumber = (Label)e.Row.FindControl("lblPhoneNumber");

            lblPhoneNumber.Text = objGeneralFunc.filPhnm(lblPhoneNumber.Text);

        }
    }

}

