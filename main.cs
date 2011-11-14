using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{

	public class App{

		private EventLevel myEventLevel = EventLevel.Information;
		private EventLevel EventLevel{
			get{return myEventLevel;}
			set{myEventLevel = value;}
		}

		public static int Main(string[] args){
			App app = new App();

			if(Array.IndexOf(args, "-v") >= 0){
				app.EventLevel = EventLevel.Verbose;
			}

			return app.Execute();
		}


		private int Execute(){
			try{
				RedFaceParser p = new RedFaceParser();
//				p.SetForceEncoding("csWindows31J");

				InitialInsertionMode temp = new InitialInsertionMode();

				string f = "data/test.html";
				FileInfo file = new FileInfo(f);

				p.ParserEventRaised += WriteEvent;

				using(FileStream fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)){
					p.Parse(fs);
				}
				var logs = p.GetLogs();
				foreach(ParserLog log in logs){
					Console.WriteLine("{0}行{1}文字: {2}", log.Line.Number, log.ColumnNumber, log.Message);
					Console.WriteLine(" {0}", log.Line.Data);
				}

				Console.WriteLine("パース開始: {0}", p.StartTime);
				Console.WriteLine("パース終了: {0}", p.EndTime);
				Console.WriteLine("パース時間: {0}", p.EndTime - p.StartTime);
				Console.WriteLine();
				Console.WriteLine("========");
				Console.WriteLine(p.Document.OuterXml);

				return 0;
			} catch(Exception e){
				Console.WriteLine(e);
				return 1;
			}
		}

		public void WriteEvent(Object sender, ParserEventArgs e){
			if(e.Level >= myEventLevel){
				Console.Write("{0}: ", e.Level);
//				Console.Write("({0}) ", sender.GetType());
				if(!string.IsNullOrEmpty(e.Message)) Console.Write(e.Message);
				Console.WriteLine();
			}
		}

	}

}



