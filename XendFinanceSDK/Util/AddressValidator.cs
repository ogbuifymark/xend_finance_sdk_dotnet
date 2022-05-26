using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace XendFinanceSDK.Util
{
    internal static class AddressValidator
    {
        public static string ValidateAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new InvalidOperationException("Invalid Address");

            AddressUtil addressUtil = new AddressUtil();

            bool isEmptyAddress = addressUtil.IsAnEmptyAddress(address);
            if (isEmptyAddress)
                throw new InvalidOperationException("Invalid Address");

            bool isCHecksumAddress = addressUtil.IsChecksumAddress(address);
            if (!isCHecksumAddress)
                address = addressUtil.ConvertToChecksumAddress(address);

            bool isAddressValid = addressUtil.IsValidAddressLength(address);
            if (!isAddressValid)
                throw new InvalidOperationException("Invalid Address");

            isAddressValid = addressUtil.IsValidEthereumAddressHexFormat(address);
            if (!isAddressValid)
                throw new InvalidOperationException("Invalid Address");

            return address;
        }
    }
}
