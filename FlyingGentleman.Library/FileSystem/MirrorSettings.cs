using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library.FileSystem
{
	/// <summary>
	/// Settings used by the File System Mirror commands.
	/// </summary>
	[DataContract]
	public class MirrorSettings
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MirrorSettings"/> class.
		/// </summary>
		public MirrorSettings()
		{
			this.IgnoreSourceDirectoryPatterns = new List<IgnoreExpression>();
			this.IgnoreSourceFileNamePatterns = new List<IgnoreExpression>();
			this.IgnoreTargetDirectoryPatterns = new List<IgnoreExpression>();
			this.IgnoreTargetFileNamePatterns = new List<IgnoreExpression>();
		}

		/// <summary>
		/// Gets or sets Regular Expressions which define the ignore patterns for files.  Files which
		/// match the pattern will not be copied to the target directory.
		/// </summary>
		/// <value>
		/// The ignore source file name patterns.
		/// </value>
		[DataMember]
		public List<IgnoreExpression> IgnoreSourceFileNamePatterns { get; set; }

		/// <summary>
		/// Gets or sets Regular Expressions which define the ignore patterns for directories.  Directories 
		/// (and other files and directories contained within the directories) which match the pattern will 
		/// not be copied to the target directory.
		/// </summary>
		/// <value>
		/// The ignore source directory patterns.
		/// </value>
		[DataMember]
		public List<IgnoreExpression> IgnoreSourceDirectoryPatterns { get; set; }

		/// <summary>
		/// Gets or sets Regular Expressions which define the ignore patterns for files.  Files which
		/// match the pattern will not be overwritten or deleted from the target directory.
		/// </summary>
		/// <value>
		/// The ignore target file name patterns.
		/// </value>
		[DataMember]
		public List<IgnoreExpression> IgnoreTargetFileNamePatterns { get; set; }

		/// <summary>
		/// Gets or sets Regular Expressions which define the ignore patterns for directories.  Directories 
		/// (and other files and directories contained within the directories) which match the pattern will 
		/// not be deleted or affected by the mirror operation.
		/// </summary>
		/// <value>
		/// The ignore target directory patterns.
		/// </value>
		[DataMember]
		public List<IgnoreExpression> IgnoreTargetDirectoryPatterns { get; set; }

        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        /// <value>
        /// The retry count.
        /// </value>
        [DataMember]
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the retry sleep seconds.
        /// </summary>
        /// <value>
        /// The retry sleep seconds.
        /// </value>
        [DataMember]
        public int RetrySleepSeconds { get; set; }

        /// <summary>
        /// Gets a MirrorSettings which ignores source SVN directories and target app_offline.htm files
        /// </summary>
        public static MirrorSettings DefaultSettings
        {
            get
            {
                return new MirrorSettings()
                {
                    IgnoreSourceDirectoryPatterns = { IgnorePatterns.SvnDirectory },
                    IgnoreTargetFileNamePatterns = { IgnorePatterns.AppOfflineFile },
                };
            }
        }
	}
}
