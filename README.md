# API - AFIP
API REST para realizar facturas electronicas ante la AFIP

<img src="https://th.bing.com/th/id/R.ef2dc632612d4b165ff4bdb3ae752ec8?rik=zyfsaTDTrThKVQ&riu=http%3a%2f%2fwww.uio.org.ar%2fwp-content%2fuploads%2f2019%2f07%2flogo-afip-900-1.png&ehk=IkQ0nVu6loguj4cWAVpiegYrCirRs9dekQwTZkeQY9c%3d&risl=&pid=ImgRaw&r=0" width="50%" height="50%">

<ol>  
    <li>Coneccion con el Web Service Soap de la AFIP  
        <ul>  
            <li>Requiere registro previo en la pagina de la afip para obtener el crt y private key</li>  
        </ul>  
    </li>  
    <li>Crea un ticket de acceso  
        <ul>  
            <li>Duracion establecida por parametros de 12h</li>
            <li>Requiere certificado digital y una llave privada</li>  
        </ul>  
    </li>  
    <li>Controladores de Facturas electronicas 
        <ul>  
            <li>Factura eletronica local</li>
            <li>Factura electronica de exportacion</li>
            <li>Ultimo comprobante emitido</li>
        </ul>  
    </li>     
</ol>  
