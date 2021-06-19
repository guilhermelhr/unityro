using System;

namespace ROIO.GRF
{
    public class GrfOpenCallback
    {
        private Action<GrfOpenCallback> callback;
        private GrfFile file;
        private int response;
        private bool returned = false;

        public GrfOpenCallback(Action<GrfOpenCallback> callback)
        {
            this.callback = callback;
        }

        public void doCallback(GrfFile file)
        {
            this.file = file;
            callback.Invoke(this);
        }

        public bool hasReturned()
        {
            return returned;
        }

        public int Response
        {
            get { return response; }
            set { response = value; returned = true; }
        }

        public GrfFile File
        {
            get { return file; }
        }
    }
}