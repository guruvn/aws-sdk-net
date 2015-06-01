/*
 * Copyright 2010-2014 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

/*
 * Do not modify this file. This file is generated from the swf-2012-01-25.normal.json service model.
 */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.IO;

using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace Amazon.SimpleWorkflow.Model
{
    /// <summary>
    /// Provides details of the <code>MarkerRecorded</code> event.
    /// </summary>
    public partial class MarkerRecordedEventAttributes
    {
        private long? _decisionTaskCompletedEventId;
        private string _details;
        private string _markerName;

        /// <summary>
        /// Gets and sets the property DecisionTaskCompletedEventId. 
        /// <para>
        /// The id of the <code>DecisionTaskCompleted</code> event corresponding to the decision
        /// task that resulted in the <code>RecordMarker</code> decision that requested this marker.
        /// This information can be useful for diagnosing problems by tracing back the chain of
        /// events leading up to this event.
        /// </para>
        /// </summary>
        public long DecisionTaskCompletedEventId
        {
            get { return this._decisionTaskCompletedEventId.GetValueOrDefault(); }
            set { this._decisionTaskCompletedEventId = value; }
        }

        // Check to see if DecisionTaskCompletedEventId property is set
        internal bool IsSetDecisionTaskCompletedEventId()
        {
            return this._decisionTaskCompletedEventId.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property Details. 
        /// <para>
        /// Details of the marker (if any).
        /// </para>
        /// </summary>
        public string Details
        {
            get { return this._details; }
            set { this._details = value; }
        }

        // Check to see if Details property is set
        internal bool IsSetDetails()
        {
            return this._details != null;
        }

        /// <summary>
        /// Gets and sets the property MarkerName. 
        /// <para>
        /// The name of the marker.
        /// </para>
        /// </summary>
        public string MarkerName
        {
            get { return this._markerName; }
            set { this._markerName = value; }
        }

        // Check to see if MarkerName property is set
        internal bool IsSetMarkerName()
        {
            return this._markerName != null;
        }

    }
}