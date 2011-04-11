<?php
ob_start();

//---------------------------------------------------
//0 -> Petición incial de refresco de la página web
//1 -> Petición del usuario (logearse)
//2 -> Petición de deslogear de usuario
//3 -> Petición de alta de un usuario
//---------------------------------------------------

if ($_POST["n"]=='2')
{
	//Borrar Cookies
	setcookie("Nick");
	setcookie("Pass");  
	$aux="'login','0'";
	//Refrescar
	header("location: connect.php");
}
//Guardar las cookies si ya ha iniciado antes la sesión
$n=$_COOKIE["Nick"];
$p=$_COOKIE["Pass"];

function Connect() 
{ 
   if (!($link=mysql_connect("db4free.net","taskuser","task747"))) 
   { 
      echo "Error #00: No a sido posible conectar a la DB."; 
      exit(); 
   } 
   if (!mysql_select_db("taskplanner",$link)) 
   { 
      echo "Error #01: No a sido posible seleccionar la DB."; 
      exit(); 
   } 
   return $link; 
} 

$link = Connect();

$pass= md5( $p );
$busqueda_sql = "SELECT * FROM USR_USUARIOS WHERE USR_NICK='".$n."' AND USR_PASSWORD='".$p."'";
$sql=mysql_query($busqueda_sql, $link) or die (mysql_error());
$total=mysql_num_rows($sql);
$admin=false;

if ($total==0)
{
	if ($_POST["n"]=='1') //1
	{
		$usuario=$_POST["user"];
		$password=$_POST["pass"];
		$pass= md5( $password );

		$busqueda_sql = "SELECT * FROM USR_USUARIOS WHERE USR_NICK='".$usuario."' AND USR_PASSWORD='".$pass."'";
		$cad=mysql_query($busqueda_sql, $link) or die (mysql_error());
		if (mysql_num_rows($cad)==0)
		{
			//--------------------------------------------------------------------------------
			// Mensaje de error al fallar user o pass
			//--------------------------------------------------------------------------------

			echo '<form action="javascript: enviar(\'login\',\'1\',\'\');" name="login" id="login">
			<div id="inputs">
				<div id="lbl_user"><label>Usuario</label></div><div id="inp_user"><input name="user" id="user" type="text" maxlength="30" /></div>
				<div id="lbl_pass"><label>Password</label></div><div id="inp_pass"><input name="pass" id="pass" type="password" maxlength="30" /></div>
				<div id="inp_enviar"><input type="submit" name="enviar" value="Enviar"/></div>
			</div>
			<div style="clear:both;"></div>
			<div id="inp_r"><label id="r" class="res">Usuario o password incorrectos</label></div>
			</form>';

			//--------------------------------------------------------------------------------
		} 
		else
		{
			//--------------------------------------------------------------------------------
			// En caso de que se loguee correctamente
			//--------------------------------------------------------------------------------

			$busqueda_sql = "SELECT * FROM USR_USUARIOS WHERE USR_NICK='".$usuario."' AND USR_TIPO=2";
			$cad=mysql_query($busqueda_sql, $link) or die (mysql_error());
			if (mysql_num_rows($cad)==0)
			{
				echo '<form action="javascript: enviar(\'login\',\'1\',\'\');" name="login" id="login">
				<div id="inputs">
					<div id="lbl_user"><label>Usuario</label></div><div id="inp_user"><input name="user" id="user" type="text" maxlength="30" /></div>
					<div id="lbl_pass"><label>Password</label></div><div id="inp_pass"><input name="pass" id="pass" type="password" maxlength="30" /></div>
					<div id="inp_enviar"><input type="submit" name="enviar" value="Enviar"/></div>
				</div>
				<div style="clear:both;"></div>
				<div id="inp_r"><label id="r" class="res">No eres administrador</label></div>
				</form>';
			}
			else
			{
				//Las cookies duran un día
				setcookie("Nick", $usuario, time()+86400);
				setcookie("Pass", $pass, time()+86400);  

				echo '<form action="javascript: enviar(\'login\',\'1\',\'\');" name="login" id="login">
				<div id="inp_r"><label>Bienvenid@ '.$usuario.'</label></div> 
				<div id="salir"><a href="javascript: enviar(\'login\',\'2\',\'\');">Cerrar sesion</a></div> 
				</form>';
				$admin=true;
			}

			//--------------------------------------------------------------------------------
		}
	}
	else //0
	{
		//--------------------------------------------------------------------------------
		// Mostrar formulario de Login
		//--------------------------------------------------------------------------------

		echo '<form action="javascript: enviar(\'login\',\'1\',\'\');" name="login" id="login">
		<div id="inputs">
		<div id="lbl_user"><label>Usuario</label></div><div id="inp_user"><input name="user" id="user" type="text" maxlength="30" /></div>
		<div id="lbl_pass"><label>Password</label></div><div id="inp_pass"><input name="pass" id="pass" type="password" maxlength="30" /></div>
		<div id="inp_enviar"><input type="submit" name="enviar" value="Enviar"/></div> 
		</div>
		<div style="clear:both;"></div>
		<div id="inp_r"><label id="r" class="res"></label></div> 
		</form>';

		//--------------------------------------------------------------------------------
	}
}
else //0 (previamente logeado)
{
	//--------------------------------------------------------------------------------
	// En caso de que esté previamente logueado
	//--------------------------------------------------------------------------------

	echo '<form action="javascript: enviar(\'login\',\'1\',\'\');" name="login" id="login">
	<div id="inp_r"><label>Bienvenid@ '.$_COOKIE["Nick"].'</label></div> 
	<div id="salir"><a href="javascript: enviar(\'login\',\'2\',\'\');">Cerrar sesion</a></div> 
	</form>';
	$admin=true;

	//--------------------------------------------------------------------------------
}

if($admin == true)
{
	//--------------------------------------------------------------------------------
	// Mostrar administrador
	//--------------------------------------------------------------------------------

	if ($_POST["n"]=='3') //3
	{
		$update_sql = "UPDATE USR_USUARIOS SET USR_ESTADO='Activo' WHERE USR_NICK='".$_POST["usr_name"]."'";
		$result=mysql_query($update_sql, $link) or die (mysql_error());
		if($result>0)
		{
			echo "<br>Usuario ".$_POST["usr_name"]." activado correctamente</br>";
		}
	}

	echo "<br><b>Usuarios inactivos:</b><br>";
	
	$busqueda_sql = "SELECT USR_NICK FROM USR_USUARIOS WHERE USR_ESTADO!='Activo'";
	$cad=mysql_query($busqueda_sql, $link) or die (mysql_error());
	if (mysql_num_rows($cad)>0)
	{
		while ($registro = mysql_fetch_array($cad))
		{
			echo $registro['USR_NICK'].' --> <a href="javascript: enviar(\'login\',\'3\',\''.$registro['USR_NICK'].'\');">Activar</a>';
		}
	}
	else
	{
		echo "No existen usuarios inactivos";
	}

	//--------------------------------------------------------------------------------
}

ob_end_flush();
?>