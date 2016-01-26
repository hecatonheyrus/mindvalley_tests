
var http=require('http');
var url=require('url');
var querystring=require('querystring');
var fs=require('fs');
var multipart=require('./multipart.js');

//---------------------------------------------------------------------------------------------------
 function parse_multipart(request){
   var parser=multipart.parser();
   parser.headers=request.headers;
   request.addListener("data",function(chunk){
     parser.write(chunk);
    });
	
	request.addListener("end",function(){
     parser.close();
    });
	
	return parser;
 }
       

var onRequest=undefined;
function start(route,handle){
   var innerObj=function (request,response) {
   var postData="";
   var pathname=url.parse(request.url).pathname;
   console.log("Request for "+pathname+" received.");
   request.setEncoding("utf8");
   
    
   request.addListener("data",function(postDataChunk){
    postData+=querystring.parse(postDataChunk).query;
	console.log("Received POST data chunk '"+querystring.parse(postDataChunk).query+"'.");
				  	  
    });


	request.addListener("end",function(){
	  	fs.writeFileSync('query',postData)
	    route(handle,pathname,response,postData);
	});


	      
 
  }
  onRequest=innerObj;
  http.createServer(onRequest).listen(9999);
  console.log("The server has started.");
}

exports.start=start;

