﻿namespace Order.exception
{
    public class IncompleteBuildException : MyException
    {
        public IncompleteBuildException(string message) : base(message) { }
    }
}