using System;

namespace com.dke.data.agrirouter.api.dto.onboard.inner
{
    /**
     * Encapsulated authentication object for endpoint communication.
     */
    public class Authentication
    {
        
        public String Type { get; set; }

        public String Secret { get; set; }

        public String Certificate { get; set; }
    }
}