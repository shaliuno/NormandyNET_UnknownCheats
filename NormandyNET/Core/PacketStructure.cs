namespace NormandyNET.Core
{
    internal class PacketStructure
    {
        private const ulong packet_magic = 0x65421546;

        private enum type
        {
            _memoryop,
            _get_mod_address,
            _complete
        }

        private struct copy_memory
        {
            private bool write;
            private uint process_id;
            private ulong address;
            private ulong size;
        }

        private struct get_base
        {
            private get_base(int charArraySize) : this()
            {
                modname = new char[256];
            }

            private uint process_id;
            private char[] modname;
        }

        private struct complete
        {
            private ulong result;
        }

        private struct header
        {
            private uint magic;
            private type type;
        }

        private struct Packet
        {
            private header header;

            private struct data
            {
                private copy_memory _memory;
                private complete _completion;
            }
        }
    }
}