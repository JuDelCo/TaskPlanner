<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<META NAME="Language" CONTENT="Spanish">
<title>Taskplanner</title>
</head>
<script language="javascript">

function POST_AJAX(url, variables) 
{
	objeto = false;
	if (window.XMLHttpRequest) 
	{ 
		objeto = new XMLHttpRequest(); //Mozilla, Safari, etc...
		if (objeto.overrideMimeType) { objeto.overrideMimeType('text/xml'); }
	} 
	else if (window.ActiveXObject) 
	{ 
		try { objeto = new ActiveXObject("Msxml2.XMLHTTP"); } //Internet Explorer
		catch (e) 
		{
			try { objeto = new ActiveXObject("Microsoft.XMLHTTP"); }
			catch (e) {}
		}
	}
	if (!objeto) 
	{
		alert("ERROR - No se ha podido crear el objeto XML-HttpRequest !!!");
		return false;
	}

	objeto.onreadystatechange = recibir_respuesta;
	objeto.open("POST", url, true);
	objeto.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
	objeto.setRequestHeader("Content-length", variables.length);
	objeto.setRequestHeader("Connection", "close");
	objeto.send(variables);
}

function enviar(id_form,n,id) //Formato de la cadena creada: "user=usuario&pass=contraseña&n=0"
{  
	if (n=='1')
	{
		document.getElementById('inp_enviar').innerHTML = '<input type="submit" name="enviar" value="Enviar"/><img src="ajax.gif"/>';
		var Formulario = document.getElementById(id_form);
		var longitudFormulario = Formulario.elements.length;
		var variables = "";
		var separarCampos = "";
		for (var i=0; i<=Formulario.elements.length-1; i++)
		{
			variables += separarCampos+Formulario.elements[i].name + '=' + encodeURI(Formulario.elements[i].value);
			separarCampos="&";
		}
		variables += '&n=' + n;
		POST_AJAX('connect.php', variables);
	}
	else
	{
		variables = 'n=' + n; 

		ID_isNullOrEmpty = true;
		if (id) ID_isNullOrEmpty = false;
            	else ID_isNullOrEmpty = true;
		if(!(ID_isNullOrEmpty)) variables += '&usr_name=' + id;

		POST_AJAX('connect.php', variables);
	}
}

function recibir_respuesta() 
{
	if ((objeto.readyState==4) && (objeto.status==200)) { document.getElementById('form').innerHTML = objeto.responseText; }
}

enviar('login','0','');

</script>

<style>

div.contenedor_general
{
	border: 1px dotted black;
	padding: 5px;
	background-color: #eeffff;
	margin: 0px auto 0px auto;
	width: 400px;
}
#tabla_form select
{
	width: 185px;
}
textarea
{
	resize: none;
}

</style>

<body>
	<div id="contenido" class="contenedor_general">
		<div id="texto"><p><b>TASKPLANNER</b></p></div>
		<div id="form"></div>
	</div>
</body>
</html>