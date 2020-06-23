using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Elements
{
    public class NeuronCoordinate
    {

        private int hash = 0;

        private int _Layer;
        private int _Row;

        public int Layer
        {
            get
            {
                return _Layer;
            }
            set
            {
                _Layer = value;
                UpdateHash();
            }
        }


        public int Row
        {
            get
            {
                return _Row;
            }
            set
            {
                _Row = value;
                UpdateHash();
            }
        }

        public NeuronCoordinate()
            :this(-1,-1)
        {

        }

        public NeuronCoordinate(int layer, int row)
        {
            this._Layer = layer;
            this._Row = row;
            UpdateHash();
        }


        private void UpdateHash()
        {
            this.hash = this.ToString().GetHashCode();
        }


        public override string ToString()
        {
            return $"{Layer}, {Row}";
        }



        public override bool Equals(object other)
        {
            NeuronCoordinate otherCoord = other as NeuronCoordinate;

            if(otherCoord == null)
            {
                return false;
            }

            return (otherCoord.GetHashCode() == this.GetHashCode());
        }

        public override int GetHashCode()
        {
            return this.hash;
        }


        public NeuronCoordinate Clone()
        {
            return new NeuronCoordinate(Layer, Row);
        }
    }
}
