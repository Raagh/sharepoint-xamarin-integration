package md511e835964478fe76dd237368797dcb5e;


public class AuthenticationAgentActivity_AdalWebViewClient
	extends android.webkit.WebViewClient
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onLoadResource:(Landroid/webkit/WebView;Ljava/lang/String;)V:GetOnLoadResource_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"n_shouldOverrideUrlLoading:(Landroid/webkit/WebView;Ljava/lang/String;)Z:GetShouldOverrideUrlLoading_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"n_shouldInterceptRequest:(Landroid/webkit/WebView;Ljava/lang/String;)Landroid/webkit/WebResourceResponse;:GetShouldInterceptRequest_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"n_onPageFinished:(Landroid/webkit/WebView;Ljava/lang/String;)V:GetOnPageFinished_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"n_onPageStarted:(Landroid/webkit/WebView;Ljava/lang/String;Landroid/graphics/Bitmap;)V:GetOnPageStarted_Landroid_webkit_WebView_Ljava_lang_String_Landroid_graphics_Bitmap_Handler\n" +
			"";
		mono.android.Runtime.register ("Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationAgentActivity+AdalWebViewClient, Microsoft.IdentityModel.Clients.ActiveDirectory.Platform, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", AuthenticationAgentActivity_AdalWebViewClient.class, __md_methods);
	}


	public AuthenticationAgentActivity_AdalWebViewClient () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AuthenticationAgentActivity_AdalWebViewClient.class)
			mono.android.TypeManager.Activate ("Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationAgentActivity+AdalWebViewClient, Microsoft.IdentityModel.Clients.ActiveDirectory.Platform, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "", this, new java.lang.Object[] {  });
	}

	public AuthenticationAgentActivity_AdalWebViewClient (java.lang.String p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == AuthenticationAgentActivity_AdalWebViewClient.class)
			mono.android.TypeManager.Activate ("Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationAgentActivity+AdalWebViewClient, Microsoft.IdentityModel.Clients.ActiveDirectory.Platform, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public void onLoadResource (android.webkit.WebView p0, java.lang.String p1)
	{
		n_onLoadResource (p0, p1);
	}

	private native void n_onLoadResource (android.webkit.WebView p0, java.lang.String p1);


	public boolean shouldOverrideUrlLoading (android.webkit.WebView p0, java.lang.String p1)
	{
		return n_shouldOverrideUrlLoading (p0, p1);
	}

	private native boolean n_shouldOverrideUrlLoading (android.webkit.WebView p0, java.lang.String p1);


	public android.webkit.WebResourceResponse shouldInterceptRequest (android.webkit.WebView p0, java.lang.String p1)
	{
		return n_shouldInterceptRequest (p0, p1);
	}

	private native android.webkit.WebResourceResponse n_shouldInterceptRequest (android.webkit.WebView p0, java.lang.String p1);


	public void onPageFinished (android.webkit.WebView p0, java.lang.String p1)
	{
		n_onPageFinished (p0, p1);
	}

	private native void n_onPageFinished (android.webkit.WebView p0, java.lang.String p1);


	public void onPageStarted (android.webkit.WebView p0, java.lang.String p1, android.graphics.Bitmap p2)
	{
		n_onPageStarted (p0, p1, p2);
	}

	private native void n_onPageStarted (android.webkit.WebView p0, java.lang.String p1, android.graphics.Bitmap p2);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
