//Generic function to call AXMX/WCF  Service
function CallService() {
    $.support.cors = true;
    $.ajaxSetup({ cache: false });
    
    $.ajax({
        type: varType, //GET or POST or PUT or DELETE verb
        url: MainURL + varUrl, // Location of the service
        data: varData, //Data sent to server
        contentType: "application/json; charset=utf-8", // content type sent to server
        dataType: "json", //Expected data format from server
        processdata: true, //True or False
        timeout: 100000,
        success: function(msg) {//On Successfull service call
            ServiceSucceeded(msg);
        },
        error: function(msg) { ServiceFailed(msg); } // When Service call fails
    });
    $.ajaxSetup({ cache: true});
}

function ServiceFailed(result) {
    alert('Service call failed: ' + result.status + '' + result.statusText);
    varType = null; varUrl = null; varData = null; varContentType = null; varDataType = null; varProcessData = null;
}


function ServiceSucceeded(result) {//When service call is sucessful

    var resultObject = null;

    $.each(result, function(key, value) {
        var p = key.toString();
        eval(p)(value);
    });    
}


function GetDealerCars(
 carMakeid, CarModalId, YearID, PriceOrderby,
             MarginRangeOrderby, NoofDayActive, AdStatus, CarActive, VehicleTypeTrim,
             InventoryID,
             DealerID) {


    varType = "GET";
    InventoryID = escape(InventoryID).replace(/\+/g, '%2b');
    varUrl = "GetDealersSearchCarsNew?carMakeid=" + carMakeid + "&CarModalId=" + CarModalId + "&YearID=" + YearID + "&PriceOrderby=" + PriceOrderby + "&MarginRangeOrderby=" + MarginRangeOrderby + "&NoofDayActive=" + NoofDayActive + "&AdStatus=" + AdStatus + "&CarActive=" + CarActive + "&VehicleTypeTrim=" + VehicleTypeTrim + "&InventoryID=" + InventoryID + "&DealerID=" + DealerID + "";
    CallService();
}



// Actions on Results
function resApply() {
    var checkedcount = 0;
    //var dum = [];
    ////console.log($('#result .grid1 tbody input[type=checkbox]:checked').length)
    ////console.log('resApply')
    if (page == 0) {
        var Count = $('#result .grid1 tbody input[type=checkbox]:checked').length;
        if (Count > 0) { showSpinner() }
        $('#result .grid1 tbody input[type=checkbox]:checked').each(function() {
            if (this.checked) {
                Count--;
                checkedcount++;
                var PostingID = $(this).attr('id');
                var AdStatusID = $('#resAction1 option:selected').val();
                ////console.log(PostingID, AdStatusID);
                DealerUpdateCarAdStatus(PostingID, AdStatusID, Count);
            }
        });
        if (checkedcount == 0) {
            alert('please select atleast one vehicle');
        }
    } else {
        showSpinner();
        var PostingID = mainPostingID;
        var AdStatusID = $('#resAction1 option:selected').val();
        DealerUpdateCarAdStatus(PostingID, AdStatusID, 1);
        var t = setTimeout(function() { location.reload(); }, 1000)

    }


}

function resDel() {
    var checkedcount = 0;
    if (page == 0) {
        var Count = $('#result .grid1 tbody input[type=checkbox]:checked').length;
        if (Count > 0) { showSpinner() }
        $('#result .grid1 tbody input[type=checkbox]:checked').each(function() {
            if (this.checked) {
                Count--;
                checkedcount++;
                var PostingID = $(this).attr('id');
                var AdStatusID = '7';
                ////console.log(PostingID, AdStatusID);
                DealerUpdateCarAdStatus(PostingID, AdStatusID, Count);
            }
        });
        if (checkedcount == 0) {
            alert('please select atleast one vehicle');
        }
    }
    else {
        showSpinner();
        var PostingID = mainPostingID;
        var AdStatusID = '7';
        if (confirm('Are you sure you want to delete this vehicle ?')) {
            DealerUpdateCarAdStatus(PostingID, AdStatusID, 1);
            var t = setTimeout(function() { location.reload(); }, 1000);
        }
    }

}


//  Search Results Binding
function GetDealersSearchCarsNewResult(value) {
debugger 

    vehicleRresults = []; ;

    $('.gridResults b').empty().text(value.length + ' vehicles')
    $('.gridHolder table tbody').empty();

    if (value.length <= 0) {
        $('.noMatch').show();
        $('#result').css('overflow', 'hidden').css('width', $(window).width()-160);
    } else {
        $('.noMatch').hide();
        $('#result').css({ 'overflow-x': 'scroll', 'overflow-y': 'hidden', 'width': $(window).width() - 160 });
    }

    $.each(value, function(key, value) {
        var m = "";
        obj = value;
        debugger 
        vehicleRresults.push(value);
        //console.log(value);
        m = '<tr class="row1">';

        m += '<td><input type="checkbox" id="' + value._postingID + '" ></td>';

        if (value._AdStatus == "Active" || value._AdStatus == "Pre Publish") {
        
            m += '<td><a href=Details.aspx?C=' + value._DealerUniqueID + ' >' + value._DealerUniqueID + '</a> </td>';

            if (value._AdStatus == "Pre Publish") {
                m += '<td>' + value._AdStatus + '</td>';
            }
            else {
                m += '<td>' + 'Published' + '</td>';

            }


        }
        else {
            m += '<td><span>' + value._DealerUniqueID + '</span> </td>';

            m += '<td>' + value._AdStatus + '</td>';
        }

        //LastUpdatedDate

        ////console.log(value._LastUpdatedDate);
        m += '<td style="text-align:right">' + getDate2(value._LastUpdatedDate) + '</td>';


        // Uploaded 
        m += '<td style="text-align:right">' + getDate(value._dateOfPosting) + '</td>';


        m += '<td>' + value._yearOfMake + '</td>';
        if (value._make.length > 15) {
            m += '<td title="' + value._make + '">' + value._make.substring(0, 15) + '..</td>';
        } else {
            m += '<td>' + value._make + '</td>';
        }

        if (value._model.length > 15) {
            m += '<td title="' + value._model + '">' + value._model.substring(0, 15) + '..</td>';
        } else {
            m += '<td>' + value._model + '</td>';
        }

        // Trim
        m += '<td></td>';

        // Body Type
        m += '<td>' + value._bodytype + '</td>';

        // Active since
        m += '<td style="text-align:right">' + value._ActiveSince + '</td>';

        // Orig Price
        m += '<td style="text-align:right" class="price10">' + value._price + '</td>';

        // Current Price
        if ((value._CurrentPrice != 'Emp') && (value._CurrentPrice != null)) {
            m += '<td style="text-align:right" class="price10">' + value._CurrentPrice + '</td>';
        } else {
            m += '<td style="text-align:right" class="price10">' + value._price + '</td>';
        }

        // Purch cos        
        if ((value._PurchaseCost != 'Emp') && (value._PurchaseCost != null)) {
            m += '<td style="text-align:right" class="price10">' + value._PurchaseCost + '</td>';
        } else {
            m += '<td style="text-align:right" ></td>';
        }

        // Gross profit
        if ((value._PurchaseCost != 'Emp') && (value._PurchaseCost != null)) {
            m += '<td style="text-align:right" class="price10">' + (value._price - value._PurchaseCost) + '</td>';
        } else {
            m += '<td style="text-align:right" ></td>';
        }


        // Pic Count
        var picCount = 0;
        for (i = 0; i < 20; i++) {
            ////console.log(value['_PIC' + i]);
            if ((value['_PIC' + i] != '') && (value['_PIC' + i] != 'Emp') && (value['_PIC' + i] != null) && (value['_PIC' + i] != undefined)) {
                picCount++;
            }
        }
        m += '<td style="text-align:right">' + picCount + '</td>';  // Pic Count



        if (value._exteriorColor && value._exteriorColor != null) {
            if (value._exteriorColor.length > 15) {
                m += '<td title="' + value._exteriorColor + '">' + value._exteriorColor.substring(0, 15) + '..</td>';
            } else if (value._exteriorColor != 'Emp') {
            m += '<td>' + value._exteriorColor + '</td>';
            } else {
                m += '<td>Unspecified</td>';
            }
        } else {
            m += '<td>Unspecified</td>';
        }
        


        if (value._interiorColor && value._interiorColor != null) {
            if (value._interiorColor.length > 15) {
                m += '<td title="' + value._interiorColor + '">' + value._interiorColor.substring(0, 15) + '..</td>';
            } else if (value._interiorColor != 'Emp') {
            m += '<td>' + value._interiorColor + '</td>';
            } else {
                m += '<td>Unspecified</td>';
            }
        } else {
            m += '<td>Unspecified</td>';
        }
        
        


        if (value._ConditionDescription && value._ConditionDescription != null) {
            if (value._ConditionDescription.length > 15) {
                m += '<td title="' + value._ConditionDescription + '">' + value._ConditionDescription.substring(0, 15) + '..</td>';
            } else if (value._ConditionDescription != 'Emp') {
                m += '<td>' + value._ConditionDescription + '</td>';
            } else {
            m += '<td>Unspecified</td>';
            }
        } else {
        m += '<td>Unspecified</td>';
        }



        // GetCarMultiSitePostings(PostingID)
        m += '<td><a class="getURLs" id=' + value._postingID + ' href=javascript:void(0);>0</a></td>';


        m += '</tr>';
        $('.gridHolder table tbody').append(m);
    });
    hideSpinner();
    
     $('#resAction1, #butApply, #butDel').attr('disabled', 'disabled');

     $('.getURLs').live('click', function() {
         GetCarMultiSitePostings($(this).attr('id'));

         var m = 'Multi-Site listing URLs for: <span>'+$(this).attr('id') + '</span><a class="close"></a>';
         $('#listingURL h3').empty().append(m);
     });
    
   
    /*
    if ($('#result table tr').length >= 2) {
    $('#result').css({ 'overflow-x': 'scroll', 'overflow-y': 'hidden' });
    } else {
    $('#result').css({ 'overflow': 'hidden'});
    }
    */
    $('.gridHolder table tbody input:checkbox').change(function() {
        if (this.checked) {
            $(this).parent().parent().addClass('select');
        } else {
            $(this).parent().parent().removeClass('select');
        }
        var $cat = $('.gridHolder table tbody');
        var len = $cat.find(":checkbox:checked").length;
        if (len > 0) {
            $('#resAction1, #butApply, #butDel').removeAttr('disabled');
        } else {
            $('#resAction1, #butApply, #butDel').attr('disabled', 'disabled');
        }

    });

    $('.gridHolder table thead input:checkbox').bind('change', function() {
        if (this.checked) {
            $('.gridHolder table tbody tr').addClass('select');
            $('.gridHolder table tbody input:checkbox').attr('checked', true);
        } else {
            $('.gridHolder table tbody tr').removeClass('select');
            $('.gridHolder table tbody input:checkbox').attr('checked', false);
        }
    });

    $('.price10').formatCurrency();

    if (value.length != 0) {


        if ($("#myTable").hasClass('tablUP')) {
            $("#myTable").removeClass('tablUP');
            $("#myTable").tablesorter();
            var sorting = [[1, 0]];
            // sort on the first column
            $("#myTable").trigger("sorton", [sorting]);
        } else {
            $("#myTable").trigger("update");
            var sorting = [[1, 0]];
            // sort on the first column
            //$("#myTable").trigger("sorton", [sorting]);
        }
    }
    //$("$('.gridHolder table.grid1").


}
/*
var aAsc = [];
function sortTable(nr) {
aAsc[nr] = aAsc[nr] == 'asc' ? 'desc' : 'asc';
$('#xtable>tbody>tr').tsort('td:eq(' + nr + ')[abbr]', { order: aAsc[nr] });
}
*/


function DealerUpdateCarAdStatusResult(result) {

    if (parseInt(result) == 0 && page == 0) {
        var p = $('.links1 li a.act').attr('id');
        var p1 = $('.links2 li a.act').attr('id');
        if (p !== undefined) {
            eval(p)();
            $('.gridHolder table thead input[type=checkbox]').attr('checked', false);
        }
        else {
            eval(p1)();
            $('.gridHolder table thead input[type=checkbox]').attr('checked', false);
        }
    }
}

// Upload & Edit Images

function editImages() {
    window.location.href = "Picsupload.aspx?CarInventoryID=" + CarID1[1] + "P" + $('#hdnDealerUniqueID').val() + " ";
}



$(function() {
    // Images Order  

    //var pic0 = pic1 = pic2 = pic3 = pic4 = pic5 = pic6 = pic7 = pic8 = pic9 = pic10 = pic11 = pic12 = pic13 = pic14 = pic15 = pic16 = pic17 = pic18 = pic19 = pic20 = 0;
$('#imageOrder, #imagesSave').click(function() {

        showSpinner();
        var dum = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];




        for (i = 0; i < $('.thumbsUpload li').length; i++) {
            dum[i] = $('.thumbsUpload li:eq(' + i + ') img.thumb').attr('picid');
        }
        //alert(PICID0);
        $('#hdnCarID').empty().val(CARID);
        //$('#hdnYear').empty().val($('#hdnYear').val());
        //$('#hdnMake').empty().val($('#hdnMake').val());
        //$('#hdnModel').empty().val($('#hdnModel').val());
        $('#hdnPic0').empty().val(PICID0);
        $('#hdnPic1').empty().val(dum[0]);
        $('#hdnPic2').empty().val(dum[1]);
        $('#hdnPic3').empty().val(dum[2]);
        $('#hdnPic4').empty().val(dum[3]);
        $('#hdnPic5').empty().val(dum[4]);
        $('#hdnPic6').empty().val(dum[5]);
        $('#hdnPic7').empty().val(dum[6]);
        $('#hdnPic8').empty().val(dum[7]);
        $('#hdnPic9').empty().val(dum[8]);
        $('#hdnPic10').empty().val(dum[9]);
        $('#hdnPic11').empty().val(dum[10]);
        $('#hdnPic12').empty().val(dum[11]);
        $('#hdnPic13').empty().val(dum[12]);
        $('#hdnPic14').empty().val(dum[13]);
        $('#hdnPic15').empty().val(dum[14]);
        $('#hdnPic16').empty().val(dum[15]);
        $('#hdnPic17').empty().val(dum[16]);
        $('#hdnPic18').empty().val(dum[17]);
        $('#hdnPic19').empty().val(dum[18]);
        $('#hdnPic20').empty().val(dum[19]);
        
        
        
        /*
        SavePictures(CARID, PICID0,
        dum[0], dum[1], dum[2], dum[3],
        dum[4], dum[5], dum[6], dum[7],
        dum[8], dum[9], dum[10], dum[11],
        dum[12], dum[13], dum[14], dum[15],
        dum[16], dum[17], dum[18], dum[19],
        '0');
        */
   
        /*
        SavePictures(sCarId, pic0,
        pic1, pic2, pic3, pic4,
        pic5, pic6, pic7, pic8,
        pic9, pic10, pic11, pic12,
        pic13, pic14, pic15, pic16,
        pic17, pic18, pic19, pic20,
        UserID);
        */
    });



    $('#backPic').click(function() {
        window.location.href = "Details.aspx?C=" + DealerUniqueID + ""
    })



})

function SavePicturesResult(result) {
    hideSpinner();
    alert('Images order updated successfully..!');
}

