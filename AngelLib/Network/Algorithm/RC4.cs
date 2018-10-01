namespace AngelLib.Network.Algorithm
{
    public class RC4
    {
        private int x = 0;

        private int y = 0;

        private byte[] state = new byte[256];

        public RC4(byte[] key)
        {
            int i;
            for (i = 0; i < 256; i++)
            {
                this.state[i] = (byte)i;
            }
            int num = 0;
            for (i = 0; i < 256; i++)
            {
                num = num + key[i % (int)key.Length] + this.state[i] & 255;
                this.swap(i, num);
            }
        }

        public void decrypt(ref byte b)
        {
            b = (byte)((b ^ this.rc4()) & 255);
        }

        public void decrypt(byte[] b)
        {
            for (int i = 0; i < (int)b.Length; i++)
            {
                b[i] = (byte)((b[i] ^ this.rc4()) & 255);
            }
        }

        internal void decrypt(byte[] b, int p, int p_2)
        {
            for (int i = p; i < p + p_2; i++)
            {
                b[i] = (byte)((b[i] ^ this.rc4()) & 255);
            }
        }

        public void decrypt_reset(byte[] b)
        {
            int num = this.x;
            int num1 = this.y;
            byte[] numArray = new byte[(int)this.state.Length];
            this.state.CopyTo(numArray, 0);
            for (int i = 0; i < (int)b.Length; i++)
            {
                b[i] = (byte)((b[i] ^ this.rc4()) & 255);
            }
            this.x = num;
            this.y = num1;
            this.state = numArray;
        }

        private int rc4()
        {
            this.x = this.x + 1 & 255;
            this.y = this.y + this.state[this.x] & 255;
            this.swap(this.x, this.y);
            int num = this.state[this.state[this.x] + this.state[this.y] & 255];
            return num;
        }

        private void swap(int i, int j)
        {
            byte num = this.state[i];
            this.state[i] = this.state[j];
            this.state[j] = num;
        }
    }
}
