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
				app.ParseArgs(args);
				return app.Execute(args);
			} catch(Exception e){
				Console.WriteLine(e);
				return 1;
			}
		}


		private int ParseFromUri(string uri){
			RedFaceParser p = new RedFaceParser();
			p.ParserEventRaised += WriteEvent;

			WebClient client = new WebClient();
			client.Headers.Add("User-Agent", "RedFace/0.1");
			using(Stream data = client.OpenRead(uri)){
				p.Parse(data);
			}
			PrintResult(p);
			return 0;
		}

		private int ParseFromFile(string path){

			FileInfo file = new FileInfo(path);
			if(!file.Exists){
				Console.WriteLine("指定されたファイルがみつかりませんでした: {0}", file.FullName);
				return 1;
			}

			RedFaceParser p = new RedFaceParser();
			p.ParserEventRaised += WriteEvent;

			using(FileStream fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)){
				p.Parse(fs);
			}
			PrintResult(p);
			return 0;
		}

		private int Execute(string[] args){
			if(myTargetPath == null){
				Console.WriteLine("対象のファイル名もしくはURLを指定してください。");
				return 1;
			}

			if(myTargetPath.StartsWith("http://") || myTargetPath.StartsWith("https://")){
				return ParseFromUri(myTargetPath);
			} else {
				return ParseFromFile(myTargetPath);
			}
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
//			Console.WriteLine(p.Document.OuterXml);
		}


		public void WriteEvent(Object sender, ParserEventArgs e){
			if(e.Level >= myEventLevel){
				Console.Write("{0}: ", e.Level);
				if(e.OriginalSender != null){
					Console.Write("{0}:", e.OriginalSender.GetType());
				}
				if(sender is RedFaceParser){
					RedFaceParser parser = (RedFaceParser)sender;
					Console.Write("({0}文字目)", parser.InputStream.CurrentPosition);
					Console.WriteLine(" {0}", parser.InputStream.GetRecentString(20));
				}
				if(!string.IsNullOrEmpty(e.Message)) Console.Write(e.Message);
				Console.WriteLine();
			}
		}


		// コマンドライン引数を解析してNameValueCollectionに格納します。
		private void ParseArgs(string[] args){
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

		}

	}

}



