﻿var Cards = new makeArray(8);
Cards[0] = new CardType("MasterCard", "51,52,53,54,55", "16");
var MasterCard = Cards[0];
Cards[1] = new CardType("VisaCard", "4", "13,16");
var VisaCard = Cards[1];
Cards[2] = new CardType("AmExCard", "34,37", "15");
var AmExCard = Cards[2];
Cards[3] = new CardType("DinersClubCard", "30,36,38", "14");
var DinersClubCard = Cards[3];
Cards[4] = new CardType("DiscoverCard", "6011", "16");
var DiscoverCard = Cards[4];
Cards[5] = new CardType("enRouteCard", "2014,2149", "15");
var enRouteCard = Cards[5];
Cards[6] = new CardType("JCBCard", "3088,3096,3112,3158,3337,3528", "16");
var JCBCard = Cards[6];
var LuhnCheckSum = Cards[7] = new CardType();

/*************************************************************************\
CheckCardNumber(form)
function called when users click the "check" button.
\*************************************************************************/
function CheckCardNumber(form) {
        
    var tmpyear;
    var valid=true ;
    $('.error').empty();
    var txt ='';
    if (form.txtCardholderName.value.length == 0) {
        txt += '<li>Please enter a <span style=color:red>card holder name</span>.</li>'
        //alert("Please enter a card holder name.");
        valid=false; 
        form.txtCardholderName.focus();
        $('#txtCardholderName').css('border','red 1px solid');
        
    }
    if (form.txtCardNumber.value.length == 0) {
        //alert("Please enter a credit card #.");
        txt += '<li>Please enter a <span style=color:red>credit card #</span>.</li>'
        $('#txtCardNumber').css('border','red 1px solid');
        valid=false;
        
    }
    if (form.ExpMon[0].selected == true) {
        //alert("Please select the expiration month.");
         txt += '<li>Please select the <span style=color:red>expiration month</span>.</li>'
         $('#ExpMon').css('border','red 1px solid');
        valid=false;
    
    }
    if (form.CCExpiresYear[0].selected == true) {
        txt += '<li>Please select the <span style=color:red>expiration year</span>.</li>';
        $('#CCExpiresYear').css('border','red 1px solid');
        valid=false;
    
    }
    
    if (form.cvv.value.length == 0) {
        txt += '<li>Please enter a <span style=color:red>cvv #</span>.</li>';
        $('#cvv').css('border','red 1px solid');
        valid=false;
    }

    var e = form.CCExpiresYear;
    
    var ExpYear = e.options[e.selectedIndex].value;

    if (e.selectedIndex>0)
    {
        if (ExpYear > 11)
            tmpyear = "20" + ExpYear;
        else if (ExpYear.value < 26)
            tmpyear = "20" + ExpYear;
        else {
           txt += '<li>The <span style=color:red>expiration year is not valid</span>.</li>';
           $('#CCExpiresYear').css('border','red 1px solid');
            valid=false;
        }
        tmpmonth = form.ExpMon.options[form.ExpMon.selectedIndex].value;
    
    
        // The following line doesn't work in IE3, you need to change it
        // to something like "(new CardType())...".
        // if (!CardType().isExpiryDate(tmpyear, tmpmonth)) {
        if (!(new CardType()).isExpiryDate(tmpyear, tmpmonth)) {
            txt += '<li>This card has <span style=color:red>already expired</span>.</li>';
             $('#CCExpiresYear,#cvv, #txtCardNumber, #ExpMon, #CCExpiresYear ').css('border','red 1px solid');
            valid=false;
        }
    }
    if (form.txtCardNumber.value.length>0 && tmpyear>0  &&tmpmonth>0&&tmpyear!='undefined' &&tmpmonth!='undefined' )
    {
    
    card = form.CardType.options[form.CardType.selectedIndex].value;
    var retval = eval(card + ".checkCardNumber(\"" + form.txtCardNumber.value +"\", " + tmpyear + ", " + tmpmonth + ");");
    cardname = "";
    if (retval){
//comment this out if used on an order form
//txt += '<li>"This card number appears to be valid.");
//return true;
                 if (form.FirstNameTextBox.value.length == 0) {
                    txt += '<li>"Please enter a <span style=color:red>billing name</span>."</li>';
                     $('#FirstNameTextBox').css('border','red 1px solid');
                    valid=false;
                
               }
               if (form.LastNameTextBox.value.length == 0) {
                    txt += '<li>"Please enter a <span style=color:red>Last name</span>."</li>';
                    $('#LastNameTextBox').css('border','red 1px solid');
                    valid=false;
                
               }
                if (form.txtBillPhone.value.length == 0) {
                txt += '<li>"Please enter a <span style=color:red>billing phone #</span>."</li>';
                $('#PhoneTextBox').css('border','red 1px solid');
                valid=false;
                
                }
                if (form.EmailTextBox.value.length == 0) {
                txt += '<li>"Please enter a <span style=color:red>EmailId </span>."</li>';
                $('#EmailTextBox').css('border','red 1px solid');
                valid=false;
                }
                if ((form.txtBillPhone.value.length > 0)&&(form.txtBillPhone.value.length < 10)&&form.txtBillPhone.value.length == 0) {
                txt += '<li>"Please enter a valid <span style=color:red>billing phone #</span>."</li>';
                 $('#PhoneTextBox').css('border','red 1px solid');
                valid=false;
                }
                 if (form.AddressTextBox.value.length == 0) {
                txt += '<li>"Please enter a <span style=color:red>billing address</span>."</li>';
                 $('#txtBillAddress').css('border','red 1px solid');
                valid=false;
                
                }
                if (form.AddressTextBox.value.length == 0) {
                txt += '<li>"Please enter a <span style=color:red>billing city</span>."</li>';
                 $('#AddressTextBox').css('border','red 1px solid');
                valid=false;
                
               }
               if (form.ddlBillState[0].selected == true) {
                txt += '<li>"Please select the <span style=color:red>billing state</span>."</li>';
                $('#ddlBillState').css('border','red 1px solid');
                valid=false;
                
               }
                if (form.txtBillZip.value.length == 0) {
                txt += '<li>"Please enter a <span style=color:red>billing zip</span>."</li>';
                 $('#txtBillZip').css('border','red 1px solid');
                form.txtBillZip.focus();
                valid=false;
               }
                if ((form.txtBillZip.value.length > 0)&&(form.txtBillZip.value.length < 4)) {
                txt += '<li>"Please enter a valid <span style=color:red>billing zip</span>."</li>';
                 $('#txtBillZip').css('border','red 1px solid');
                valid=false;
               }
               
               
         }      
             
               
    }
    else {
    
    if (form.txtCardNumber.value.length>0 && tmpyear>0  &&tmpmonth>0&&tmpyear!='undefined' &&tmpmonth!='undefined'  )
    {
        // The cardnumber has the valid luhn checksum, but we want to know which
        // cardtype it belongs to.
        for (var n = 0; n < Cards.size; n++) {
            if (Cards[n].checkCardNumber(form.txtCardNumber.value, tmpyear, tmpmonth)) {
                cardname = Cards[n].getCardType();
                break;
            }
        }
        if (cardname.length > 0) {
            txt += '<li>"This looks like a ' + cardname + ' number, not a ' + card + ' number."</li>';
            valid=false;
        }
        else {
            txt += '<li>"This card number is not valid."</li>';
            $('#txtCardNumber').css('border','red 1px solid');
            valid=false;
        }
        
    }
        
          
    }
    
    if (valid==false)  
        {
                $('.error').append(txt);
                $('.errHolder').show();
                return valid; 
                
           }
           else 
           
           {
               $('.error').empty();
               $('.errHolder').hide();
           }
}



/*************************************************************************\
Object CardType([String cardtype, String rules, String len, int year, 
int month])
cardtype    : type of card, eg: MasterCard, Visa, etc.
rules       : rules of the cardnumber, eg: "4", "6011", "34,37".
len         : valid length of cardnumber, eg: "16,19", "13,16".
year        : year of expiry date.
month       : month of expiry date.
eg:
var VisaCard = new CardType("Visa", "4", "16");
var AmExCard = new CardType("AmEx", "34,37", "15");
\*************************************************************************/
function CardType() {
    var n;
    var argv = CardType.arguments;
    var argc = CardType.arguments.length;

    this.objname = "object CardType";

    var tmpcardtype = (argc > 0) ? argv[0] : "CardObject";
    var tmprules = (argc > 1) ? argv[1] : "0,1,2,3,4,5,6,7,8,9";
    var tmplen = (argc > 2) ? argv[2] : "13,14,15,16,19";

    this.setCardNumber = setCardNumber;  // set CardNumber method.
    this.setCardType = setCardType;  // setCardType method.
    this.setLen = setLen;  // setLen method.
    this.setRules = setRules;  // setRules method.
    this.setExpiryDate = setExpiryDate;  // setExpiryDate method.

    this.setCardType(tmpcardtype);
    this.setLen(tmplen);
    this.setRules(tmprules);
    if (argc > 4)
        this.setExpiryDate(argv[3], argv[4]);

    this.checkCardNumber = checkCardNumber;  // checkCardNumber method.
    this.getExpiryDate = getExpiryDate;  // getExpiryDate method.
    this.getCardType = getCardType;  // getCardType method.
    this.isCardNumber = isCardNumber;  // isCardNumber method.
    this.isExpiryDate = isExpiryDate;  // isExpiryDate method.
    this.luhnCheck = luhnCheck; // luhnCheck method.
    return this;
}

/*************************************************************************\
boolean checkCardNumber([String cardnumber, int year, int month])
return true if cardnumber pass the luhncheck and the expiry date is
valid, else return false.
\*************************************************************************/
function checkCardNumber() {
    var argv = checkCardNumber.arguments;
    var argc = checkCardNumber.arguments.length;
    var cardnumber = (argc > 0) ? argv[0] : this.cardnumber;
    var year = (argc > 1) ? argv[1] : this.year;
    var month = (argc > 2) ? argv[2] : this.month;

    this.setCardNumber(cardnumber);
    this.setExpiryDate(year, month);

    if (!this.isCardNumber())
        return false;
    if (!this.isExpiryDate())
        return false;

    return true;
}
/*************************************************************************\
String getCardType()
return the cardtype.
\*************************************************************************/
function getCardType() {
    return this.cardtype;
}
/*************************************************************************\
String getExpiryDate()
return the expiry date.
\*************************************************************************/
function getExpiryDate() {
    return this.month + "/" + this.year;
}
/*************************************************************************\
boolean isCardNumber([String cardnumber])
return true if cardnumber pass the luhncheck and the rules, else return
false.
\*************************************************************************/
function isCardNumber() {
    var argv = isCardNumber.arguments;
    var argc = isCardNumber.arguments.length;
    var cardnumber = (argc > 0) ? argv[0] : this.cardnumber;
    if (!this.luhnCheck())
        return false;

    for (var n = 0; n < this.len.size; n++)
        if (cardnumber.toString().length == this.len[n]) {
        for (var m = 0; m < this.rules.size; m++) {
            var headdigit = cardnumber.substring(0, this.rules[m].toString().length);
            if (headdigit == this.rules[m])
                return true;
        }
        return false;
    }
    return false;
}

/*************************************************************************\
boolean isExpiryDate([int year, int month])
return true if the date is a valid expiry date,
else return false.
\*************************************************************************/
function isExpiryDate() {
    var argv = isExpiryDate.arguments;
    var argc = isExpiryDate.arguments.length;

    year = argc > 0 ? argv[0] : this.year;
    month = argc > 1 ? argv[1] : this.month;

    if (!isNum(year + ""))
        return false;
    if (!isNum(month + ""))
        return false;
    today = new Date();
    expiry = new Date(year, month);
    if (today.getTime() > expiry.getTime())
        return false;
    else
        return true;
}

/*************************************************************************\
boolean isNum(String argvalue)
return true if argvalue contains only numeric characters,
else return false.
\*************************************************************************/
function isNum(argvalue) {
    argvalue = argvalue.toString();

    if (argvalue.length == 0)
        return false;

    for (var n = 0; n < argvalue.length; n++)
        if (argvalue.substring(n, n + 1) < "0" || argvalue.substring(n, n + 1) > "9")
        return false;

    return true;
}

/*************************************************************************\
boolean luhnCheck([String CardNumber])
return true if CardNumber pass the luhn check else return false.
Reference: http://www.ling.nwu.edu/~sburke/pub/luhn_lib.pl
\*************************************************************************/
function luhnCheck() {
    var argv = luhnCheck.arguments;
    var argc = luhnCheck.arguments.length;

    var CardNumber = argc > 0 ? argv[0] : this.cardnumber;

    if (!isNum(CardNumber)) {
        return false;
    }

    var no_digit = CardNumber.length;
    var oddoeven = no_digit & 1;
    var sum = 0;

    for (var count = 0; count < no_digit; count++) {
        var digit = parseInt(CardNumber.charAt(count));
        if (!((count & 1) ^ oddoeven)) {
            digit *= 2;
            if (digit > 9)
                digit -= 9;
        }
        sum += digit;
    }
    if (sum % 10 == 0)
        return true;
    else
        return false;
}

/*************************************************************************\
ArrayObject makeArray(int size)
return the array object in the size specified.
\*************************************************************************/
function makeArray(size) {
    this.size = size;
    return this;
}

/*************************************************************************\
CardType setCardNumber(cardnumber)
return the CardType object.
\*************************************************************************/
function setCardNumber(cardnumber) {
    this.cardnumber = cardnumber;
    return this;
}

/*************************************************************************\
CardType setCardType(cardtype)
return the CardType object.
\*************************************************************************/
function setCardType(cardtype) {
    this.cardtype = cardtype;
    return this;
}

/*************************************************************************\
CardType setExpiryDate(year, month)
return the CardType object.
\*************************************************************************/
function setExpiryDate(year, month) {
    this.year = year;
    this.month = month;
    return this;
}

/*************************************************************************\
CardType setLen(len)
return the CardType object.
\*************************************************************************/
function setLen(len) {
    // Create the len array.
    if (len.length == 0 || len == null)
        len = "13,14,15,16,19";

    var tmplen = len;
    n = 1;
    while (tmplen.indexOf(",") != -1) {
        tmplen = tmplen.substring(tmplen.indexOf(",") + 1, tmplen.length);
        n++;
    }
    this.len = new makeArray(n);
    n = 0;
    while (len.indexOf(",") != -1) {
        var tmpstr = len.substring(0, len.indexOf(","));
        this.len[n] = tmpstr;
        len = len.substring(len.indexOf(",") + 1, len.length);
        n++;
    }
    this.len[n] = len;
    return this;
}

/*************************************************************************\
CardType setRules()
return the CardType object.
\*************************************************************************/
function setRules(rules) {
    // Create the rules array.
    if (rules.length == 0 || rules == null)
        rules = "0,1,2,3,4,5,6,7,8,9";

    var tmprules = rules;
    n = 1;
    while (tmprules.indexOf(",") != -1) {
        tmprules = tmprules.substring(tmprules.indexOf(",") + 1, tmprules.length);
        n++;
    }
    this.rules = new makeArray(n);
    n = 0;
    while (rules.indexOf(",") != -1) {
        var tmpstr = rules.substring(0, rules.indexOf(","));
        this.rules[n] = tmpstr;
        rules = rules.substring(rules.indexOf(",") + 1, rules.length);
        n++;
    }
    this.rules[n] = rules;
    return this;
}
//  End -->

