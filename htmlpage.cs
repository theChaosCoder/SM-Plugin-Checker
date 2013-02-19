using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SM_Plugin_Checker
{
    static class htmlpage
    {
        public const string Header = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                                    <html xmlns='http://www.w3.org/1999/xhtml'><head>
                                    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                                    <meta http-equiv='X-UA-Compatible' content='IE=10' >
                                    <style type='text/css'>
                                    * {
	                                    padding: 0px;
	                                    margin: 0px;
                                    }
                                    body {
	                                    color: #FFF;
	                                    font-family: Arial, Helvetica, sans-serif;
	                                    font-size: 9pt;
	                                    font-weight: bold;
	                                    background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAEECAIAAABmzcnZAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAPxJREFUeNqMzwlugzAQBdAfwr7vO4QAyRVa9f73omIsL5CqwtKTPeM/loyf7y8NgLBt26Gm3r6f+ufcOcPq/bxdzmz8vUuZC71/Msd6v1f+czHzOfNnDu/XKq0rXmSRlgXrMkvzjIU8hfnJTcw04UkewvTgRvIYVYMwDlxPhp7rSN+pWtK1XEPaRlWj2dVcRepKVaLalUdlWaAsVDmKXS7leYY8k7IsZVIpTROkiZQkMRMzMYkQR1IUhUwohSRAGDCB4CPwffiCB99jPMGF57pwBQeu48BR2TaxBQu2ZcFSmSbMDwZMw4Ch0nXoJ/f7/UjToJ3cbrcDZf0KMACpbF6A1SlhAgAAAABJRU5ErkJggg==);
	                                    background-repeat: repeat-x;
                                    }
                                    a {
                                        text-decoration: none;
                                    }
                                    td {
	                                    text-align:center;
	                                    background: #262626;

                                    }
                                    .green, .red, .orange {
                                        border: medium none;
                                        border-radius: 2px 2px 2px 2px;
                                        color: #FFFFFF !important;
                                        padding: 6px;
                                        text-decoration: none;
	                                    text-align: center;
                                    }
                                    .green {
	                                    background: #A4D007;
                                    }
                                    .red {
	                                    background: #ad4548;
                                    }
                                    .orange {
	                                    background: #4F4FFF;
                                    }
                                    #head {
	                                    _position: fixed	
                                    }
                                    #data {
	                                    _margin-top: 25px;

                                    }
                                    .rightalign {
	                                    text-align: left;
	                                    padding-left: 8px;
                                        padding-right: 8px;
                                    }
                                    .hasTooltip span {
                                        display: none;
                                        color: #000;
                                        text-decoration: none;
                                        padding: 3px;
                                        text-align: left;
                                    }

                                    .hasTooltip:hover span {
                                        display: block;
                                        position: absolute;
                                        background-color: #FFF;
                                        border: 1px solid #CCC;
                                        margin: 10px -20px;
                                    }
                                    .progress_bar {
                                        position: absolute;
                                        top: 40%;
                                        left: 10%;
                                        margin: 0 0 15px 0;
                                        width: 80%;
                                        border: 2px solid #262626;
                                        background: #ad4548;  
                                        border-radius: 6px;
                                    }
 
                                    .progress_bar span {
                                      position: absolute;
                                      left: 0;
                                      top: 0;
                                      line-height: 50px;
                                      background-color: #A4D007;
                                      border-radius: 6px;
                                    }
 
                                    .progress_bar strong {
                                      position: relative;
                                      z-index: 10;
                                      display: block;
                                      line-height: 50px;
                                      text-align: center;
                                    }
                                    </style>

                                    <script language=JavaScript>
                                    <!--
                                    var message="""";
                                    function clickIE() {if (document.all) {(message);return false;}}
                                    document.oncontextmenu=new Function(""return false"")
                                    // --> 
                                    </script>

                                    </head>







                                    <body bgcolor='black' oncontextmenu=""return false;"">
<!--
<script type=""text/javascript"">
document.write(""<br>So, so, Sie verwenden also "" + navigator.appCode);
document.write(""<br>So, so, Sie verwenden also "" + navigator.appName+""<br>"");
document.write(navigator.userAgent);
</script> // --> 



";

        //#CEAC24
        public const string Footer = "&nbsp;</body></html>";

        public static string progressbar(int prog)
        {
            return Header + "<div class='progress_bar'><strong>" + prog.ToString() + " %</strong><span style='width: " + prog.ToString() + "%;'>&nbsp;</span></div>" + Footer;
        }
    }
}
