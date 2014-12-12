namespace APNGLibrary
{
    /// <summary>
    /// Chunk type enumeration
    /// </summary>
	public enum ChunkType
	{
        /// <summary>
        /// Header
        /// </summary>
		IHDR,

        /// <summary>
        /// Data
        /// </summary>
		IDAT,

        /// <summary>
        /// End
        /// </summary>
		IEND,

        /// <summary>
        /// Text
        /// </summary>
		tEXt,

        /// <summary>
        /// 
        /// </summary>
		rtSv,

        /// <summary>
        /// Animation control (APNG)
        /// </summary>
		acTL,

        /// <summary>
        /// Transparency
        /// </summary>
		tRNS,

        /// <summary>
        /// Frame control (APNG)
        /// </summary>
		fcTL,

        /// <summary>
        /// Frame data (APNG)
        /// </summary>
		fdAT
	}
}

