﻿using JKLog.Configuration;
using JKLog.Interface;
using System;
using System.Collections;
using System.Collections.Generic;



namespace JKLog
{
    public class JKReader : IEnumerable<IEntry>, IDisposable
    {
        private IReadable mapper;



        public JKReader(IReadable mapper)
        {
            this.mapper = mapper;
        }



        #region IEnumerable

        public IEnumerator<IEntry> GetEnumerator()
        {
            return this.mapper.Entries;
        }



        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion



        #region IDisposable

        /// <summary>
        /// Disposes mapper safely.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }



        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.mapper != null)
                {
                    MapperManager.DisposeMapper(this.mapper);
                    this.mapper = null;
                }
            }
        }

        #endregion
    }
}
