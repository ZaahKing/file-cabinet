namespace FileCabinetApp.Parser
{
    /// <summary>
    /// Describe executavble bool operator like '=', '>', '!=' and others.
    /// </summary>
    internal interface IExpressionBoolOperator
    {
        /// <summary>
        /// Execution method.
        /// </summary>
        /// <returns>REsult of bool operation.</returns>
        public bool Execute();
    }
}
