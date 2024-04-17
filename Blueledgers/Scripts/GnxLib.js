// JScript File
var windowpopup;
var tmpGridViewRowBGColor;
var appName = "BlueLedger";
//var xmlReq  = getXmlHTTP();

// Use for OnDblClick event on GridViewRow was raised to redirect page to destiantion on the assigned target
function GridView_Row_DblClick(destinationURL, target)
{
    document.location.target = target;
    document.location.href   = destinationURL;
}

function GridView_Row_MouseOver(rowobject)
{
    // Backup class name
    tmpGridViewRowBGColor           = rowobject.style.backgroundColor;    
    rowobject.style.backgroundColor = "#EBEBEB";
}

// User for OnMouseOut event on GridViewRow was raised.
function GridView_Row_MouseOut(rowobject)
{   
    // Restore class name
    rowobject.style.backgroundColor = tmpGridViewRowBGColor;
}

//// Use for delete row GridView
//function GridView_Row_Delete(grdName, deletedRow)
//{
//    if (confirm('Do you want to delete this row?'))
//    {   
//        __doPostBack(grdName,"Delete$" + deletedRow);
//    }
//}

// To hide/show submenu when click on main menu.
function RootMenu_Click(subMenuID)
{
    var subMenu = document.getElementById(subMenuID);
    
    if (subMenu.style.display == "" || subMenu.style.display == "block")
    {
        subMenu.style.display = "none";
    }
    else
    {
        subMenu.style.display = "block";
    }
}

// To display page that relate to seleted menu
function ChildMenu_Click(navigateURL)
{
    top.main.document.location.href = navigateURL;
}

// Display setup page and setup menu
function Setup_Click()
{
    top.main.location.href = "../Setup/SetupList.aspx";
    top.menu.location.href = "../Menu/Menu.aspx?action=setup" ;
} 

//// For tab click event
//function Tab_Click(mainLink)
//{   
//    top.menu.document.location.href = "../Menu/Menu.aspx?action=menu"
//    top.main.document.location.href = mainLink;
//}

// Logout from application
function Logout_Click()
{
    top.document.location.href = "../Login.aspx";
}

// On close the popup page if any
function body_OnFocus()
{   
    if (windowpopup != null)
    {
        windowpopup.close();
    }
}

// Open account list popup
function btn_AccountCode_Click(accountCodeObject, accountNameObject, accountNatureObject, accountDrObject, accountCrObject)
{
    var name     = "lookup";
    var features = "toolbar=no;menubar=no;status=no"
    windowpopup  = window.open('/' + appName + '/GL/Account/AccountLookup.aspx?accountCodeObject=' 
                   + accountCodeObject + '&accountNameObject=' + accountNameObject + '&accountNatureObject=' 
                   + accountNatureObject + '&accountDrObject=' + accountDrObject + '&accountCrObject=' + accountCrObject, name, features);    
    windowpopup.focus();
    
    return false;
}

// Open account list popup
function btn_CountryCode_Click(countryCodeObject, countryNameObject)
{
    var name     = "lookup";
    var features = "toolbar=no;menubar=no;status=no"
    windowpopup  = window.open('/' + appName + '/Reference/Account/AccountLookup.aspx?countryCodeObject=' 
                                   + countryCodeObject + '&countryNameObject=' + countryNameObject,name,features);
                 
    windowpopup.focus();
    
    return false;
}

// Set shortcut to initial style
function Shortcut_OnMouseOut(shortcut)
{
    shortcut.className = "TBL_SHORTCUT";
}

// Set shortcut to mouser over style
function Shortcut_OnMouseOver(shortcut)
{
    shortcut.className = "TBL_SHORTCUT_OVER";
}

// Display page on main frame from selected shortcut
function Shortcut_Click(url)
{
    top.menu.document.location.href = "../Menu/Menu.aspx?action=menu"
    top.main.document.location.href = url;
}

//Get XMLHTTP for IE or Mozilla
function getXmlHTTP()
{
	if(typeof XMLHttpRequest != "undefined"){
		return new XMLHttpRequest();
	}
	else if(typeof ActiveXObject != "undefined")
	{
		try{
			var xmlhttp= new ActiveXObject("Microsoft.XMLHTTP");
			return xmlhttp;
		}
		catch(e)
		{
			return null;
		}
	}
	return null;
}

//Function to Abort previous request
function abortXmlHTTP(objXmlRequest)
{	    
	if(objXmlRequest != null)
	{			
	    objXmlRequest.abort();
	}
}

// Check unique value in database
//function DBUniqueClientValidate(source, arguments)
//{
//    var fieldID = source.getAttribute("FieldID");  
//    
//    // Create XMLHttpRequest Object
//    var xmlReq = getXmlHTTP();
//    
//    // Abort previous XMLHttpRequest object
//    abortXmlHTTP(xmlReq);
//    
//    //Assign client script to onreadystatechange
//	xmlReq.onreadystatechange = function()
//	{
//	    //When request alreay finish, show search result in textbox
//	    //readyState = 4 : The data transfer has been complete.			    
//        if(xmlReq.readyState == 4){    
//                    
//            //If found nothing, not do anything.		            
//		    if(xmlReq.responseXML == null || xmlReq.responseXML.firstChild == null)
//		    {		        
//		        arguments.IsValid = false;    
//		        return;
//		    }

//		    // Get return data from responseXML
//		    var uniqued = unescape(xmlReq.responseXML.firstChild.getAttribute("uniqued"));		    
//		    
//		    if (uniqued == "")
//		    {		       
//		        arguments.IsValid = false;
//		        return;
//		    }
//            
//		    // Set validation result
//		    if (uniqued == "True")
//		    {
//		        arguments.IsValid = true;
//		    }
//		    else
//		    {   		        
//		        arguments.IsValid = false;		        
//		    }		    
//	    }
//	}
//	
//	//Use server-side script without refresh page in a browser.
//    //Open a connection to our ASPX page with the specified query to get the results asynch.    
//    xmlReq.open("GET","/" + appName + "/Scripts/AJAX_DBUniqueValidate.aspx?fieldid=" + fieldID + "&value=" + arguments.Value , true);
//	
//    //Send the request.
//    xmlReq.send(null);
//}

function DBUniqueClientValidate(objInput)
{
    var fieldID         = objInput.getAttribute("fieldid");  
    var value           = objInput.value;
    var errormessage    = objInput.getAttribute("errormessage");
   
    // Create XMLHttpRequest Object
    var xmlReq = getXmlHTTP();
    
    // Abort previous XMLHttpRequest object
    abortXmlHTTP(xmlReq);
    
    //Assign client script to onreadystatechange
	xmlReq.onreadystatechange = function()
	{
	    //When request alreay finish, show search result in textbox
	    //readyState = 4 : The data transfer has been complete.			    
        if(xmlReq.readyState == 4){    
            
            //If found nothing, not do anything.		            
		    if(xmlReq.responseXML == null || xmlReq.responseXML.firstChild == null)
		    {		        
		        alert(errormessage);    
		        objInput.value = "";
		        objInput.focus();
		        return;
		    }

		    // Get return data from responseXML
		    var uniqued = unescape(xmlReq.responseXML.firstChild.getAttribute("uniqued"));		    
		    
		    if (uniqued == "")
		    {	
		        alert(errormessage);
		        objInput.value = "";
		        objInput.focus();
		        return;
		    }
            
		    // Set validation result
		    if (uniqued == "False")
		    {		        
		        alert(errormessage);
		        objInput.value = "";
		        objInput.focus();
		    }		    
	    }
	}
	
	//Use server-side script without refresh page in a browser.
    //Open a connection to our ASPX page with the specified query to get the results asynch.    
    xmlReq.open("GET","/" + appName + "/Scripts/AJAX_DBUniqueValidate.aspx?fieldid=" + fieldID + "&value=" + value , true);
	
    //Send the request.
    xmlReq.send(null);
}

// Function for FarPoint SpreadForWeb 3.0 ---------------------------------------------------------
    
function btnOver(theTD,ftbName,imageOver,imageDown) 
{
    event.srcElement.style.backgroundColor = "#E0E0E0";        
}

function btnOut(theTD,ftbName,imageOver,imageDown) 
{
    event.srcElement.style.backgroundColor = ""; 
}

function setFocus(ss) 
{    
    if (document.all != null) 
    {
        ss.focus();
    } 
    else 
    {
        the_fpSpread.SetPageActiveSpread(ss);
        the_fpSpread.Focus(ss);
    }
}

function FontBold(fpSpreadID) 
{        
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("FontBold");
    setFocus(ss);
}

function FontItalic(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("FontItalic");
    setFocus(ss);
}

function SetFontName(name,fpSpreadID) 
{    
    if (document.all != null) document.body.focus();
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("FontName."+name);
    setFocus(ss);
}

function SetFontSize(size,fpSpreadID) 
{
    if (document.all!=null) document.body.focus();

    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("FontSize."+size);
    setFocus(ss);
}

function FontUnderline(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("FontUnderline");
    setFocus(ss);
}

function AlignLeft(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("AlignLeft");
    setFocus(ss);
}

function AlignCenter(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("AlignCenter");
    setFocus(ss);
}

function AlignRight(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("AlignRight");
    setFocus(ss);
}   

function AlignFull(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("AlignFull");
    setFocus(ss);
} 

function AlignTop(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("AlignTop");
    setFocus(ss);
} 

function AlignMiddle(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("AlignMiddle");
    setFocus(ss);
} 

function AlignBottom(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("AlignBottom");
    setFocus(ss);
}
  
function SetForeColor(color,fpSpreadID) 
{
    if (document.all!=null) document.body.focus();
    
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("ForeColor."+color);
    setFocus(ss);
}

function SetBackColor(color,fpSpreadID) 
{
    if (document.all!=null) document.body.focus();
    
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("BackColor."+color);
    setFocus(ss);
}

function Cut(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.Clear();
}

function Copy(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.Copy();
}

function Paste(fpSpreadID) 
{
    var ss = document.getElementById(fpSpreadID);
    ss.Paste();
}

function Merge(fpSpreadID)
{
    var ss = document.getElementById(fpSpreadID);
    ss.CallBack("Merge");
    setFocus(ss);
}

function menu_over(center, cback, left, lback, right, rback, submenu, submenuParent)
{
	var objLeft    = document.getElementById(left);
	var objCenter  = document.getElementById(center);
	var objRight   = document.getElementById(right);	
	
	objLeft.style.backgroundImage   = objLeft.style.backgroundImage.replace('menu_border_left', lback);
	objCenter.style.backgroundImage = objCenter.style.backgroundImage.replace('menu_border_center', cback);
	objRight.style.backgroundImage  = objRight.style.backgroundImage.replace('menu_border_right', rback);
			
	if (submenu != '')
	{
		var objSubMenu          = document.getElementById(submenu);
		var objSubMenuParent    = document.getElementById(submenuParent);
		
		objSubMenu.style.display = '';
		
		if (objSubMenu.offsetWidth < objSubMenuParent.offsetWidth)
		{
		    objSubMenu.style.width = objSubMenuParent.offsetWidth;
		}
	}
}

function menu_out(center, cback, left, lback, right, rback, submenu)
{
	var objLeft    = document.getElementById(left);
	var objCenter  = document.getElementById(center);
	var objRight   = document.getElementById(right);

	objLeft.style.backgroundImage   = objLeft.style.backgroundImage.replace(lback, 'menu_border_left');
	objCenter.style.backgroundImage = objCenter.style.backgroundImage.replace(cback, 'menu_border_center');
	objRight.style.backgroundImage  = objRight.style.backgroundImage.replace(rback, 'menu_border_right');
	
	if (submenu != '')
	{
		var objSubMenu = document.getElementById(submenu);			
		
		objSubMenu.style.display = 'none';
	}
}

function menu_overfav(submenu,submenuParent)
{
	if (submenu != '')
	{
		var objSubMenu          = document.getElementById(submenu);
		var objSubMenuParent    = document.getElementById(submenuParent);
		
		objSubMenu.style.display = '';
		
		if (objSubMenu.offsetWidth < objSubMenuParent.offsetWidth)
		{
		    objSubMenu.style.width = objSubMenuParent.offsetWidth;
		}
	}
}

function menu_outfav(submenu)
{
	
	if (submenu != '')
	{
		var objSubMenu = document.getElementById(submenu);			
		
		objSubMenu.style.display = 'none';
	}
}


function collapse_click(objButton,objShowHideID)
{
	var objShowHide = document.getElementById(objShowHideID);
	
	if (objShowHide.style.display == 'none')
	{
		objButton.src               = objButton.src.replace('expand','collapse')
		objShowHide.style.display   = '';		
	}
	else
	{
		objButton.src               = objButton.src.replace('collapse','expand')
		objShowHide.style.display   = 'none';
	}	
	
	return false;
}

/***** Command Bar *******************************************************************************/
var timeoutHandle;
var timeout = 1000;

function ShowSubMenu(id) {
    // Hide all submenu
    HideAllSubMenu()

    var subMenu = document.getElementById(id);
    subMenu.style.display = 'block';

    setTimeOut(id);
}

function HideAllSubMenu() {
    var subMenus = document.getElementById('ul_control').getElementsByTagName('ul');

    for (var i = 0; i < subMenus.length; i++) {
        subMenus[i].style.display = 'none';
    }
}

function HideSubMenu(id) {
    var submenu = document.getElementById(id);
    submenu.style.display = 'none';
}

function SubMenuItemMouserOver(item) {
    claearTimeOut();
}

function SubMenuItemMouserOut(item) {
    setTimeOut(item.parentNode.id);
}

function setTimeOut(id) {
    timeoutHandle = setTimeout("HideSubMenu('" + id + "')", timeout);
}

function claearTimeOut() {
    clearTimeout(timeoutHandle);
}
/*************************************************************************************************/

function TXT_NUM_V1_OnKeyDown() {
    // Number
    if (event.keyCode >= 48 && event.keyCode <= 57) {
        return true;
    }

    // Number (numpad)
    if (event.keyCode >= 96 && event.keyCode <= 105) {
        return true;
    }

    // Decimal Point (".")
    if (event.keyCode == 110 || event.keyCode == 190) {
        return true;
    }

    // Left, Right, Delete, Back Space and Tab
    if (event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9) {
        return true;
    }

    return false;
}