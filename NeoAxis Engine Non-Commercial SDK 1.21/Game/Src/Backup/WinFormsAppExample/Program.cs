// Copyright (C) 2006-2012 NeoAxis Group Ltd.
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinFormsAppExample
{
	/// <summary>
	/// Defines an input point in the application.
	/// </summary>
	static class Program
	{
		[DllImport( "user32.dll" )]
		static extern bool SetProcessDPIAware();

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			if( Environment.OSVersion.Version.Major >= 6 )
			{
				try
				{
					SetProcessDPIAware();
				}
				catch { }
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new MainForm() );
		}
	}
}