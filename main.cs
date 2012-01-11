using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{

	public class App{

		private EventLevel myEventLevel = EventLevel.Information;
		private EventLevel EventLevel{
			get{return myEventLevel;}
			set{myEventLevel = value;}
		}
		private NameValueCollection myArgs = new NameValueCollection();
		private string myTargetPath = null;


		public static int Main(string[] args){
			try{
				App app = new App();
				return app.Execute(args);
			} catch(Exception e){
				Console.WriteLine(e);
				return 1;
			}
		}


		private void ParseFromUri(string uri){
			RedFaceParser p = new RedFaceParser();
			p.ParserEventRaised += WriteEvent;

			WebClient client = new WebClient();
			client.Headers.Add ("User-Agent", "RedFace/1.0");
			using(Stream data = client.OpenRead(uri)){
				p.Parse(data);
			}
			PrintResult(p);
		}

		private void ParseFromFile(string path){
			RedFaceParser p = new RedFaceParser();
			p.ParserEventRaised += WriteEvent;

			FileInfo file = new FileInfo(path);
			using(FileStream fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)){
				p.Parse(fs);
			}
			PrintResult(p);
		}

		private int Execute(string[] args){
			for(int i=0; i < args.Length; i++){
				string argName = args[i];
				if(argName.StartsWith("-")){
					if(argName == "-v"){
						this.EventLevel = EventLevel.Verbose;
					}
				} else {
					myTargetPath = argName;
				}
			}

			if(myTargetPath == null){
				Console.WriteLine("対象のファイル名もしくはURLを指定してください。");
				return 1;
			}

			if(myTargetPath.StartsWith("http://")){
				ParseFromUri(myTargetPath);
			} else {
				ParseFromFile(myTargetPath);
			}
			return 0;
		}

		private void PrintResult(RedFaceParser p){
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



