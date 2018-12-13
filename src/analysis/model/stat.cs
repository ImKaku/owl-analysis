namespace OwlAnalysis.Model{
    public class Stat{
        public string Id{get; set;}

        public string Key{get; set;}
        
        public string Value{get; set;}

        public Stat(){
        }

        public Stat(string key, string value){
            this.Key = key;
            this.Value = value;
        }
    }
}