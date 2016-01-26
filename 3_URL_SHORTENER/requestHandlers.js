var querystring=require('querystring');
var fs=require('fs');
var readline=require('readline');
var http=require('http');



function start(response,postData){
 console.log("Request handler START was called.");

 response.writeHead(200,{'Content-Type': 'text/html; charset=utf-8'});
 response.write("<meta charset=\"utf-8\">");
 response.write("<title>The URL shortener</title>");
   

    
 response.write("<style type=\"text/css\">");
 response.write(".srch_query{font-family: \"Arial Black\",Arial,sans-serif; font-size: 3em;"+
                " margin-left:150px;margin-top:250px; }");
				
response.write(".search_txt{border: 1px solid #CCCCCC; border-radius: 2px; height: 26px;}");				
	

response.write(".fileform {background-color: #FFFFFF;border: 1px solid #CCCCCC; border-radius: 2px;"+
               "cursor: pointer; height: 20px; overflow: visible; padding: 2px; position: relative;"+
			   "text-align: right; vertical-align: middle; width: 200px;margin-left: 220px;}");
			   
response.write(".fileform .selectbutton {background-color: #A2A3A3; border: 1px solid #939494; border-radius: 2px;"+
               "color: #FFFFFF; float: left; font-size: 14px; height: 18px; line-height: 20px;"+
               "overflow: hidden; text-align: center; vertical-align: middle; width: 65px;}");
			   
response.write(".fileform #upload-file { position: absolute; top:0; left:0; width:100%;-moz-opacity: 0;"+
               "filter: alpha(opacity=0); opacity:0; font-size: 150px; height: 30px;z-index:20; }");
			   
response.write(".fileform .uploadbutton {background-color: #FFFFFF; border: 1px solid #A0A0A0; border-radius: 2px;"+
               "color: #FFFFFF; float: right; font-size: 14px; height: 20px; line-height: 20px;"+
               "overflow: visible; padding: 2px 6px; text-align: center; vertical-align: middle; width: 65px;}");		   	  			   		                 			   
response.write("</style>");
    

 response.write("<body>");
 
 response.write("<div id=\"searchContainer\" class=\"srch_query\">");
 response.write("<form  method=\"post\" action=\"/upload\" id=\"searchForm\" onsubmit=\"onSearchSubmit(event)\">");
 response.write("<input type=\"text\"  class=\"search_txt\" name=\"query\" size=\"120\" maxlength=\"256\" autofocus=\"autofocus\"/>");
 response.write("</form>");

 response.write("</body>");                

 response.end();
 
  }
       

 function upload(response,data){
 
 console.log("Request handler UPLOAD was called.");
 var queryObj=querystring.parse(data);
 console.log("data received:"+data);
    
 var rd = readline.createInterface({
   input: fs.createReadStream('./database.dat'),
   output: process.stdout,
   terminal: false
 });

 rd.on('line', function(line){
 	var key_value = line.split(',');
 	if(key_value[0]===data)
      console.log(key_value[0]+":"+key_value[1]);

     response.writeHead(200,{"Content-Type":"text/html"});
        response.write("<head>");
        response.write("<meta charset=\"utf-8\">");


        response.write("<script language=\"javascript\">");	
        response.write("window.onload=function(){window.location='"+key_value[1]+"';}");
        response.write("</script>");	

        response.write("</head>");
     
        response.write("<body>");	
        response.write("URL shortener");
        response.write("</body>");	

      response.end();
     
     
});
 	  
  
 }
 
 exports.start=start;
 exports.upload=upload;