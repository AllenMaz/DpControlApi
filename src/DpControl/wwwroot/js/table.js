function findObj(theObj, theDoc) {
	var p, i, foundObj;
	if (!theDoc) theDoc = document;
	if ((p = theObj.indexOf("?")) > 0 && parent.frames.length) {
		theDoc = parent.frames[theObj.substring(p + 1)].document;
		theObj = theObj.substring(0, p);
	}
	if (!(foundObj = theDoc[theObj]) && theDoc.all)
		foundObj = theDoc.all[theObj];
	for (i = 0; !foundObj && i < theDoc.forms.length; i++) foundObj =

		theDoc.forms[i][theObj];
	for (i = 0; !foundObj &&
		theDoc.layers && i < theDoc.layers.length; i++) foundObj = findObj(theObj, theDoc.layers[i].document);
	if (!foundObj && document.getElementById)
		foundObj = document.getElementById(theObj);
	return foundObj;
}

//添加一个列
//count = 1;

function AddNewColumn() {
	// var link = document.createElement('link');
	//    link.type = 'text/css';
	//    link.rel = 'stylesheet';
	//    link.href = 'css/style.default.css';
	//    document.getElementsByTagName("head")[0].appendChild(link);
	var txtTDLastIndex = findObj("txtTDLastIndex", document);
	var columnID = parseInt(txtTDLastIndex.value);

	var tab = document.getElementById("tab");
	var rowLength = tab.rows.length;
	var columnLength = tab.rows[1].cells.length;

	for (var i = 0; i < rowLength; i++) {
		if (i == 0) {
			var oTd = tab.rows[0].insertCell(columnLength);
			oTd.id = "column" + columnID;
			txtTDLastIndex.value = (columnID + 1).toString();
			oTd.innerHTML = "<div class=\"input-append\"><input type=\"time\" style=\"height:22px;width:65px \" id=\"time" + columnID + "\" onblur=\"testdate(" + columnID + ") \" value=\"\"/><a href='javascript:' onclick=\"DeleteSignColumn('column" + columnID + "')\"><span class=\"close\">&times;</span></a></div>";
			//<span class=\"add-on\"><i class=\"icon-remove\"></i></span>onmouseout=\"testdate("+columnID+")\"
		} else if (i > 0) {
			var oTd = tab.rows[i].insertCell(columnLength);
			//          oTd.id = "column" + columnID;
			//       oTd.innerHTML = "<input type=\"number\" id=\"spinner\" name=\""+columnID+"\" class=\"input-small input-spinner\" maxlength=\"3\" style=\"width: 50px; margin-right: 16px; text-align: right;value=\"0\"; min=\"0\" ; max=\"100\">0</input>";
			oTd.innerHTML = '<input type="number" min="-1" max="100" value="-1" style="width:65px ; text-align: center" maxlength="3"></input>';
			// 	oTd.innerHTML=' <span class="field"><input type="text" id="spinner" name="" class="input-small input-spinner" /></span>';	
		}
	}

}
//添加一个行
var index = 1;

function AddNewRow() {
	var txtTRLastIndex = findObj("txtTRLastIndex", document);
	var rowID = parseInt(txtTRLastIndex.value);

	var tab = findObj("tab", document);
	var columnLength = tab.rows[0].cells.length;

	//添加行
	var newTR = tab.insertRow(tab.rows.length);
	newTR.id = "SignItem" + rowID;

	for (var i = 0; i < columnLength; i++) {
		if (i == 0) { //第一列:序号
			newTR.insertCell(0).innerHTML = ++index - 1;
		} else if (i > 0 && i < 7) {
			newTR.insertCell(i).innerHTML = "<input id='textrow" + rowID + "col" + i + "' type='text' style='border: 0px;width:auto' size='12'  />";
		} else if (i >= 7) {
			newTR.insertCell(i).innerHTML = "<input id='textItem0' type='text' style='border: 0px;width:70%' size='12' /><a class='btn btn-primary btn-circle' style='margin-left: 10px;' href='#myModal' data-toggle='modal'><i class='iconfa-plus icon-white'></i></a>";

		}
	}

	//添加列:删除按钮
	var lastTd = newTR.insertCell(columnLength);
	lastTd.innerHTML = "<div align='center' style='width:40px'><a href='javascript:' onclick=\"DeleteSignRow('SignItem" + rowID + "')\">删除</a></div>";

	//将行号推进下一行
	txtTRLastIndex.value = (rowID + 1).toString();

		var current = rowID.toString();
		var Before = rowID - 1;
		var BeforeID = Before.toString();
		
		for (var j = 1; j < 7; j++) {

			document.getElementById("textrow" + current + "col" + j).value =document.getElementById("textrow" + BeforeID + "col" + j).value

		}
}

//删除指定行
function DeleteSignRow(rowid) {
	var tab = findObj("tab", document);
	var signItem = findObj(rowid, document);

	//获取将要删除的行的Index
	var rowIndex = signItem.rowIndex;

	//删除指定Index的行
	tab.deleteRow(rowIndex);


	//重新排列序号，如果没有序号，这一步省略
	for (i = 2; i < tab.rows.length; i++) {
		tab.rows[i].cells[0].innerHTML = i - 1;
	}

	--index
}

//删除指定列
function DeleteSignColumn(columnId) {
	var tab = document.getElementById("tab");
	var columnLength = tab.rows[0].cells.length;

	var column = document.getElementById(columnId).cellIndex;

	//删除指定单元格 
	for (var i = 0; i < tab.rows.length; i++) {
		tab.rows[i].deleteCell(column);
	}

	//重新排列序号，如果没有序号，这一步省略
	//var column = columnLength - 4;

	//for (var j = 1; j < column; j++) {
	//    tab.rows[1].cells[j + 3].innerHTML = "<div style='background: #D3E6FE;width=100%;'>" + j + "</div>";
	//}

	// --count;
}


//清空列表
function ClearAllSign() {
	if (confirm('确定要清空所有吗？')) {
		index = 0;
		var tab = findObj("tab", document);
		var rowscount = tab.rows.length;

		//循环删除行,从最后一行往前删除
		for (i = rowscount - 1; i > 1; i--) {
			tab.deleteRow(i);
		}

		//重置最后行号为1
		var txtTRLastIndex = findObj("txtTRLastIndex", document);
		txtTRLastIndex.value = "1";

		//预添加一行
		AddNewRow();
	}
}