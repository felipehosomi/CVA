using CVA.IntegracaoMagento.SalesOrderStatus.Controller;
using CVA.IntegracaoMagento.SalesOrderStatus.Models.Magento;
using CVA.IntegracaoMagento.SalesOrderStatus.Models.SAP;
using Flurl.Http;
using Newtonsoft.Json;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.SalesOrderStatus
{
    public class Integration
    {
        internal static SLConnection SLConnection;
        private static readonly string ServiceLayerURL = ConfigurationManager.AppSettings["ServiceLayerURL"];
        internal static readonly string Database = ConfigurationManager.AppSettings["Database"];
        private static readonly string B1User = ConfigurationManager.AppSettings["B1User"];
        private static readonly string B1Password = ConfigurationManager.AppSettings["B1Password"];

        public Integration()
        {
        }

        //public string DecodeStringFromBase64(string stringToDecode)
        //{
        //    return Encoding.UTF8.GetString(Convert.FromBase64String(stringToDecode));
        //}

        public async Task SetIntegration()
        {
            //string sTeste = DecodeStringFromBase64("PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz48Y3RlUHJvYyB2ZXJzYW89IjMuMDAiIHhtbG5zPSJodHRwOi8vd3d3LnBvcnRhbGZpc2NhbC5pbmYuYnIvY3RlIj48Q1RlIHhtbG5zPSJodHRwOi8vd3d3LnBvcnRhbGZpc2NhbC5pbmYuYnIvY3RlIj48aW5mQ3RlIElkPSJDVGUzNTE5MDYxMjMyOTk4NTAwMDE4MjU3MDAxMDAwMTAyNzExMTY2MzE4ODc3MCIgdmVyc2FvPSIzLjAwIj48aWRlPjxjVUY+MzU8L2NVRj48Y0NUPjY2MzE4ODc3PC9jQ1Q+PENGT1A+NjM1MzwvQ0ZPUD48bmF0T3A+UFJFU1RBQ0FPIERFIFNFUlZJQ08gREUgVFJBTlNQT1JURSBBIEVTVEFCRUxFQ0lNRU5UTyBDT01FUkNJPC9uYXRPcD48bW9kPjU3PC9tb2Q+PHNlcmllPjE8L3NlcmllPjxuQ1Q+MTAyNzExPC9uQ1Q+PGRoRW1pPjIwMTktMDYtMDVUMTk6Mjg6NDktMDM6MDA8L2RoRW1pPjx0cEltcD4xPC90cEltcD48dHBFbWlzPjE8L3RwRW1pcz48Y0RWPjA8L2NEVj48dHBBbWI+MTwvdHBBbWI+PHRwQ1RlPjA8L3RwQ1RlPjxwcm9jRW1pPjA8L3Byb2NFbWk+PHZlclByb2M+QnJ1ZGFtIC0gUm9kb0FlcmVvPC92ZXJQcm9jPjxjTXVuRW52PjM1NTAzMDg8L2NNdW5FbnY+PHhNdW5FbnY+U0FPIFBBVUxPPC94TXVuRW52PjxVRkVudj5TUDwvVUZFbnY+PG1vZGFsPjAxPC9tb2RhbD48dHBTZXJ2PjA8L3RwU2Vydj48Y011bkluaT4zNTUwMzA4PC9jTXVuSW5pPjx4TXVuSW5pPlNBTyBQQVVMTzwveE11bkluaT48VUZJbmk+U1A8L1VGSW5pPjxjTXVuRmltPjUyMDg3MDc8L2NNdW5GaW0+PHhNdW5GaW0+R09JQU5JQTwveE11bkZpbT48VUZGaW0+R088L1VGRmltPjxyZXRpcmE+MTwvcmV0aXJhPjxpbmRJRVRvbWE+MTwvaW5kSUVUb21hPjx0b21hMz48dG9tYT4wPC90b21hPjwvdG9tYTM+PC9pZGU+PGNvbXBsPjx4RW1pPk1BUkNFTE8gR09OQ0FMVkVTIERPPC94RW1pPjxmbHV4bz48eE9yaWc+Q0dIPC94T3JpZz48eERlc3Q+R1lOPC94RGVzdD48L2ZsdXhvPjxFbnRyZWdhPjxjb21EYXRhPjx0cFBlcj4yPC90cFBlcj48ZFByb2c+MjAxOS0wNi0wNTwvZFByb2c+PC9jb21EYXRhPjxjb21Ib3JhPjx0cEhvcj4zPC90cEhvcj48aFByb2c+MTg6MDA6MDA8L2hQcm9nPjwvY29tSG9yYT48L0VudHJlZ2E+PG9yaWdDYWxjPkNHSDwvb3JpZ0NhbGM+PGRlc3RDYWxjPkdZTjwvZGVzdENhbGM+PHhPYnM+ODIzIHwgVE9ETyBDRU5UUk8gREUgQ1VTVE8gREVWRVJBIENPTlNUQVIgTk8gQ1QtRSB8IEVNIEFURU5ESU1FTlRPIEEgTEVJIERBIFRSQU5TUEFSRU5DSUEgRklTQ0FMIDEyLjc0MS8yMDEyIE8gVE9UQUwgREUgVFJJQlVUT1MgKDEyLDkzJSkgUiQgMjEsMDEgfCBNRCAxNjE3ODggfCBERVNUSU5POiBHWU4gfCBPUklHRU06IENHSDwveE9icz48T2JzQ29udCB4Q2FtcG89Im1pbnV0YSI+PHhUZXh0bz4wMDAwMTYxNzg4PC94VGV4dG8+PC9PYnNDb250PjxPYnNDb250IHhDYW1wbz0iUkFNTyI+PHhUZXh0bz5SQ0ZEQzwveFRleHRvPjwvT2JzQ29udD48T2JzQ29udCB4Q2FtcG89InJvdGEiPjx4VGV4dG8+fEdZTjwveFRleHRvPjwvT2JzQ29udD48T2JzQ29udCB4Q2FtcG89IlJFU1BTRUciPjx4VGV4dG8+NDwveFRleHRvPjwvT2JzQ29udD48L2NvbXBsPjxlbWl0PjxDTlBKPjEyMzI5OTg1MDAwMTgyPC9DTlBKPjxJRT4xNDczNjYxMjgxMTk8L0lFPjx4Tm9tZT5LQVJHQSBGQUNJTCBMT0dJU1RJQ0EgRSBUUkFOU1BPUlRFIExUREE8L3hOb21lPjx4RmFudD5LQVJHQSBGQUNJTDwveEZhbnQ+PGVuZGVyRW1pdD48eExncj5BVi4gRFIuIExJTk8gREUgTU9SQUVTIExFTUU8L3hMZ3I+PG5ybz4yODA8L25ybz48eEJhaXJybz5KRC4gQUVST1BPUlRPPC94QmFpcnJvPjxjTXVuPjM1NTAzMDg8L2NNdW4+PHhNdW4+U0FPIFBBVUxPPC94TXVuPjxDRVA+MDQzNjAwMDA8L0NFUD48VUY+U1A8L1VGPjxmb25lPjExMjM4NjcwMDc8L2ZvbmU+PC9lbmRlckVtaXQ+PC9lbWl0PjxyZW0+PENOUEo+MDc3NDg4MzcwMDAxNjI8L0NOUEo+PElFPjExNzE4NTY2ODExMDwvSUU+PHhOb21lPk1FVFJPSE0gUEVOU0FMQUIgSU5TVFJVTUVUQUNBTyBBTkFMSVRJQ0EgTFREQTwveE5vbWU+PHhGYW50Pk1FVFJPSE0gUEVOU0FMQUIgSU5TVFJVTUVUQUNBTyBBTkFMSVRJQ0EgTFREQTwveEZhbnQ+PGVuZGVyUmVtZT48eExncj5SVUEgTUlORVJWQSwgMTY3PC94TGdyPjxucm8+Uy9OPC9ucm8+PHhCYWlycm8+UEVSRElaRVM8L3hCYWlycm8+PGNNdW4+MzU1MDMwODwvY011bj48eE11bj5TQU8gUEFVTE88L3hNdW4+PENFUD4wNTUwNzAzMDwvQ0VQPjxVRj5TUDwvVUY+PGNQYWlzPjEwNTg8L2NQYWlzPjx4UGFpcz5CcmFzaWw8L3hQYWlzPjwvZW5kZXJSZW1lPjwvcmVtPjxkZXN0PjxDTlBKPjYwNDk4NzA2MDM3MDc3PC9DTlBKPjxJRT4xMDQ4NzY3MzU8L0lFPjx4Tm9tZT5DQVJHSUxMIEZPT0RTIEJSQVpJTDwveE5vbWU+PGVuZGVyRGVzdD48eExncj5SVUEgSVpBIENPU1RBIC0gUEFSVEUgRCAtIDE8L3hMZ3I+PG5ybz4wU048L25ybz48eEJhaXJybz5DSEFDQVJBUyBSRVRJUk88L3hCYWlycm8+PGNNdW4+NTIwODcwNzwvY011bj48eE11bj5HT0lBTklBPC94TXVuPjxDRVA+NzQ2NjUzMjA8L0NFUD48VUY+R088L1VGPjxjUGFpcz4xMDU4PC9jUGFpcz48eFBhaXM+QnJhc2lsPC94UGFpcz48L2VuZGVyRGVzdD48L2Rlc3Q+PHZQcmVzdD48dlRQcmVzdD4xNzcuMTU8L3ZUUHJlc3Q+PHZSZWM+MTc3LjE1PC92UmVjPjxDb21wPjx4Tm9tZT5GUkVURSBQRVNPPC94Tm9tZT48dkNvbXA+MTc3LjE1PC92Q29tcD48L0NvbXA+PC92UHJlc3Q+PGltcD48SUNNUz48SUNNUzAwPjxDU1Q+MDA8L0NTVD48dkJDPjE3Ny4xNTwvdkJDPjxwSUNNUz43LjAwPC9wSUNNUz48dklDTVM+MTIuNDA8L3ZJQ01TPjwvSUNNUzAwPjwvSUNNUz48L2ltcD48aW5mQ1RlTm9ybT48aW5mQ2FyZ2E+PHZDYXJnYT40ODY3Ljc5PC92Q2FyZ2E+PHByb1ByZWQ+TUVESUNBTUVOVE9TPC9wcm9QcmVkPjx4T3V0Q2F0PkNBLlBBUEVMQU88L3hPdXRDYXQ+PGluZlE+PGNVbmlkPjAxPC9jVW5pZD48dHBNZWQ+UEVTTyBCUlVUTzwvdHBNZWQ+PHFDYXJnYT4xLjA3MDA8L3FDYXJnYT48L2luZlE+PGluZlE+PGNVbmlkPjAzPC9jVW5pZD48dHBNZWQ+Vk9MVU1FUzwvdHBNZWQ+PHFDYXJnYT4xLjAwMDA8L3FDYXJnYT48L2luZlE+PHZDYXJnYUF2ZXJiPjQ4NjcuNzk8L3ZDYXJnYUF2ZXJiPjwvaW5mQ2FyZ2E+PGluZkRvYz48aW5mTkZlPjxjaGF2ZT4zNTE5MDYwNzc0ODgzNzAwMDE2MjU1MDAxMDAwMDUzNzM4MTAwMDMwNDY3MTwvY2hhdmU+PC9pbmZORmU+PC9pbmZEb2M+PGluZk1vZGFsIHZlcnNhb01vZGFsPSIzLjAwIj48cm9kbz48Uk5UUkM+NDUyOTQ3Mjc8L1JOVFJDPjwvcm9kbz48L2luZk1vZGFsPjwvaW5mQ1RlTm9ybT48YXV0WE1MPjxDTlBKPjA2MzY3OTUzMDAwMTc5PC9DTlBKPjwvYXV0WE1MPjwvaW5mQ3RlPjxTaWduYXR1cmUgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPjxTaWduZWRJbmZvPjxDYW5vbmljYWxpemF0aW9uTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvVFIvMjAwMS9SRUMteG1sLWMxNG4tMjAwMTAzMTUiLz48U2lnbmF0dXJlTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnI3JzYS1zaGExIi8+PFJlZmVyZW5jZSBVUkk9IiNDVGUzNTE5MDYxMjMyOTk4NTAwMDE4MjU3MDAxMDAwMTAyNzExMTY2MzE4ODc3MCI+PFRyYW5zZm9ybXM+PFRyYW5zZm9ybSBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNlbnZlbG9wZWQtc2lnbmF0dXJlIi8+PFRyYW5zZm9ybSBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnL1RSLzIwMDEvUkVDLXhtbC1jMTRuLTIwMDEwMzE1Ii8+PC9UcmFuc2Zvcm1zPjxEaWdlc3RNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjc2hhMSIvPjxEaWdlc3RWYWx1ZT5mVm9EN0JDSE5OdXZ0ZC9yM2VzTjdkZ2hzcEU9PC9EaWdlc3RWYWx1ZT48L1JlZmVyZW5jZT48L1NpZ25lZEluZm8+PFNpZ25hdHVyZVZhbHVlPkdiRGhlZXVGQll0cURZa0JzeHBXY1diODdLbVdZZGFUOGFqWUw5RzVDTk5FdGFrODNyQ3p3QWV2Z24rVElmZHhQd1cvS01PcXZQOEltMmR2MWVQOEM1SHZHdG1HcFhpUll6Z0JYVmpmNXQ1ekpqMElXbXZrajRjYmZXUzl4WVNrbEg2TDNpQVFpOW1UaU1salNMVkNOUkRyc05TcXNhVzB6WWU0cUprOVdGQ3VEdkFDeWhxVXBxT2pFNE9WVXVEZDRpaUFna01oZ0s5NWZlVnN2UnpQUWVxSGRkNXJHK1pQcWl2MGhXVkJzamQ0bTdNSkZBSkJER3EwYnZUdGM4cXVGcTVGUFkxNlZkSlFuemJMOUlWTUt4LzJIcWZkL1VrQVJySTErcW04TUxVQ0Q5NHNTK3ArMW50MmdxQ1BrKzZpei80aTRSTGFWQWhQcVFyM0VvYnNKdz09PC9TaWduYXR1cmVWYWx1ZT48S2V5SW5mbz48WDUwOURhdGE+PFg1MDlDZXJ0aWZpY2F0ZT5NSUlIK1RDQ0JlR2dBd0lCQWdJUVJWYUhWcEkvSjVPVVhFVjFNQllneVRBTkJna3Foa2lHOXcwQkFRc0ZBREIxTVFzd0NRWURWUVFHRXdKQ1VqRVRNQkVHQTFVRUNoTUtTVU5RTFVKeVlYTnBiREUyTURRR0ExVUVDeE10VTJWamNtVjBZWEpwWVNCa1lTQlNaV05sYVhSaElFWmxaR1Z5WVd3Z1pHOGdRbkpoYzJsc0lDMGdVa1pDTVJrd0Z3WURWUVFERXhCQlF5QlRTVTVEVDFJZ1VrWkNJRWMxTUI0WERURTVNRE15T1RFNE5ETXdNVm9YRFRJd01ETXlPREU0TkRNd01Wb3dnZmt4Q3pBSkJnTlZCQVlUQWtKU01STXdFUVlEVlFRS0RBcEpRMUF0UW5KaGMybHNNUXN3Q1FZRFZRUUlEQUpUVURFU01CQUdBMVVFQnd3SlUyRnZJRkJoZFd4dk1UWXdOQVlEVlFRTERDMVRaV055WlhSaGNtbGhJR1JoSUZKbFkyVnBkR0VnUm1Wa1pYSmhiQ0JrYnlCQ2NtRnphV3dnTFNCU1JrSXhGakFVQmdOVkJBc01EVkpHUWlCbExVTk9VRW9nUVRFeElqQWdCZ05WQkFzTUdVRjFkR1Z1ZEdsallXUnZJSEJ2Y2lCQlVpQkVZWE5qYUdreFFEQStCZ05WQkFNTU4wdEJVa2RCSUVaQlEwbE1JRXhQUjBsVFZFbERRU0JGSUZSU1FVNVRVRTlTVkVWVElFeFVSRUU2TVRJek1qazVPRFV3TURBeE9ESXdnZ0VpTUEwR0NTcUdTSWIzRFFFQkFRVUFBNElCRHdBd2dnRUtBb0lCQVFDNE5wa1dSMXR6dFo1NlN5R215cDlMVGEwODllQWVqR1d6T0xCdERpRWs0RnJPZXdUU3hOdTFrdkVXMHdNbWtQZVlFMW1ZaHJreDJWWUtiWElmSXR4ejFoNG9jWlU2UTNsZ1dnQTI2aDVGVG16M3BRUmdDa0hqcFRrcmlBY2VXSWRGQnMxRkh3Y2dHVXc3VnZPVkliRmFtbDRDQ2ZwcTFWdEJzV0Y3OWlnK3NxQlNhRmxkNDFpYlV3RnAvYU80VmExQUd0NXhmTzlyOURPZllzT3YxZ3l4QkRXSFpLL1BxdlMySjk4Y1Y4L0hpTllldGk4YnFpMW9RL0ZLc2NjalhWOVRQdnNrUnloMTZMQnNSL1BSdS9teFlwS3ErNUVSZ3V2MkVRU1k4eWJJM3p6cVp0VTFQcW5zSGEwK21PVWRFODc3Q2xVTnlDTUdIaWdRU0xwdFdPaEZBZ01CQUFHamdnTCtNSUlDK2pDQndRWURWUjBSQklHNU1JRzJvRDBHQldCTUFRTUVvRFFFTWpFNU1EWXhPVGN5TVRRM056VXdOVEk0T0Rnd01EQXdNREF3TURBd01EQXdNREF3TURJNE16STJOelkwTVZOVFVGTlFvQ1VHQldCTUFRTUNvQndFR2tWRVNVeEZWVnBCSUVOUFRrTkZTVU5CVHlCRVJTQk5SVXhQb0JrR0JXQk1BUU1Eb0JBRURqRXlNekk1T1RnMU1EQXdNVGd5b0JjR0JXQk1BUU1Ib0E0RUREQXdNREF3TURBd01EQXdNSUVhWldScGJHVjFlbUZBYTJGeVoyRm1ZV05wYkM1amIyMHVZbkl3Q1FZRFZSMFRCQUl3QURBZkJnTlZIU01FR0RBV2dCUm41MElSdnZJNEJoVkpENGYvMmdYZVhZNlVJakI0QmdOVkhTQUVjVEJ2TUcwR0JtQk1BUUlCSERCak1HRUdDQ3NHQVFVRkJ3SUJGbFZvZEhSd09pOHZhV053TFdKeVlYTnBiQzVoWTNOcGJtTnZjaTVqYjIwdVluSXZjbVZ3YjNOcGRHOXlhVzh2WkhCakwwRkRYMU5KVGtOUFVsOVNSa0l2UkZCRFgwRkRYMU5KVGtOUFVsOVNSa0l1Y0dSbU1JRzJCZ05WSFI4RWdhNHdnYXN3VktCU29GQ0dUbWgwZEhBNkx5OXBZM0F0WW5KaGMybHNMbU5sY25ScGMybG5iaTVqYjIwdVluSXZjbVZ3YjNOcGRHOXlhVzh2YkdOeUwwRkRVMGxPUTA5U1VrWkNSelV2VEdGMFpYTjBRMUpNTG1OeWJEQlRvRkdnVDRaTmFIUjBjRG92TDJsamNDMWljbUZ6YVd3dWIzVjBjbUZzWTNJdVkyOXRMbUp5TDNKbGNHOXphWFJ2Y21sdkwyeGpjaTlCUTFOSlRrTlBVbEpHUWtjMUwweGhkR1Z6ZEVOU1RDNWpjbXd3RGdZRFZSMFBBUUgvQkFRREFnWGdNQjBHQTFVZEpRUVdNQlFHQ0NzR0FRVUZCd01DQmdnckJnRUZCUWNEQkRDQnBRWUlLd1lCQlFVSEFRRUVnWmd3Z1pVd1d3WUlLd1lCQlFVSE1BS0dUMmgwZEhBNkx5OXBZM0F0WW5KaGMybHNMbUZqYzJsdVkyOXlMbU52YlM1aWNpOXlaWEJ2YzJsMGIzSnBieTlqWlhKMGFXWnBZMkZrYjNNdlFVTmZVMGxPUTA5U1gxSkdRbDlITlM1d04yTXdOZ1lJS3dZQkJRVUhNQUdHS21oMGRIQTZMeTl2WTNOd0xXRmpMWE5wYm1OdmNpMXlabUl1WTJWeWRHbHphV2R1TG1OdmJTNWljakFOQmdrcWhraUc5dzBCQVFzRkFBT0NBZ0VBUnVlZE8zOXhIcTRaaUN0Q20rM3BJRUp6QlNBTjNXVEpSY2ZhWDdLV3FVUGtsNXpaRnJ1U3RtMXBjOXJGeVA3c2RBMDNGQmxaZWlUVS9TZ2x0Z1pQODZ4S053ZkNacjVHTEROV2trcDVBOVlNOCt2TmFoZDhIQlFnWnRJKysxRFZ1TDNPQmdSMTluUUY5aExtN2NmYmdJZnpQNU5zN3hwV0t4M01aMlc1MVhQRE83cHROa2JGbTVCQU50VTAvU2ZsbHdFWjhHbmRUVU55L0x6S0NtcEd2VVdReHh3YUxyMXpUWlFiS3VpdWFEaGRVWlJ4UkxPWmZaSGZBVlh1dkE4dURDZDdzMXd4KzNCN0ZNNElpV0RJUGVPNU5NRUYyYlJIMTY0UitFZnI4UUNVd1BiQllncWcrd0xnQThVZFF5TzVMVGF6L2xRa3B4N3pwZUNjeGg4NjZDakcvRHZtQURWTURHd2c5eHZlRnZMN0kvWmlsMU1pL0NFdUFMNndOQ3dwU0VBcXFJUXpCYk9rR1BwNjN1WE1lbVllZnk4TksvNjYzWG1ZeEpnc3BIN2NYY0g4M1VuL3I1SVdScWNGWkE0MXEzaTY5aFM4TDlPOTBTNGtGZXlMOTlzWVNkSU53S1hCUkZmOFNVOUZYNTgvcXVzSGFDL3JIYjlKZVFzLzgrQWFJdWwxQ0ZUckxSOVoyNzlKTkVJRlF1KzNkZ1BzYkU3aWlZalg4SmVZVDEva2R1OUxsT1FvanZKMXlsZ21EME1CQ0ZJdlJveFZENUw4T3NDNnZQM1IvQmpRRG1xVDMyNUlVaXZqWmdXV3Q5azZ0azEvVnRoK0d5Q2hUSHA2eXltYmFKTHdmcUdKT1NyZU5CVnNsMXBRZW5JWFJDZHdaWkdtS1JuYXZvN2JtN2c9PC9YNTA5Q2VydGlmaWNhdGU+PC9YNTA5RGF0YT48L0tleUluZm8+PC9TaWduYXR1cmU+PC9DVGU+PHByb3RDVGUgdmVyc2FvPSIzLjAwIj48aW5mUHJvdCB4bWxucz0iaHR0cDovL3d3dy5wb3J0YWxmaXNjYWwuaW5mLmJyL2N0ZSI+PHRwQW1iPjE8L3RwQW1iPjx2ZXJBcGxpYz5TUC1DVGUtMzAtMDEtMjAxOTwvdmVyQXBsaWM+PGNoQ1RlPjM1MTkwNjEyMzI5OTg1MDAwMTgyNTcwMDEwMDAxMDI3MTExNjYzMTg4NzcwPC9jaENUZT48ZGhSZWNidG8+MjAxOS0wNi0wNVQxOToyODo1MC0wMzowMDwvZGhSZWNidG8+PG5Qcm90PjEzNTE5MTQ5MzQ3MDIxNDwvblByb3Q+PGRpZ1ZhbD5mVm9EN0JDSE5OdXZ0ZC9yM2VzTjdkZ2hzcEU9PC9kaWdWYWw+PGNTdGF0PjEwMDwvY1N0YXQ+PHhNb3Rpdm8+QXV0b3JpemFkbyBvIHVzbyBkbyBDVC1lPC94TW90aXZvPjwvaW5mUHJvdD48L3Byb3RDVGU+PC9jdGVQcm9jPg==");

            DateTime dataAtual = DateTime.Now.AddSeconds(1);
            string sCaminho = String.Format(@"{0}Log", System.AppDomain.CurrentDomain.BaseDirectory.ToString());

            if (!(System.IO.Directory.Exists(sCaminho)))
                System.IO.Directory.CreateDirectory(sCaminho);

            sCaminho = String.Format(@"{0}\\Log_{1}.txt", sCaminho, String.Format(@"{0}{1}{2}", dataAtual.Year.ToString(), dataAtual.Month.ToString().PadLeft(2, '0'), dataAtual.Day.ToString().PadLeft(2, '0')));

            if (!(System.IO.File.Exists(sCaminho)))
                System.IO.File.Create(sCaminho).Close();

            Util.GravarLog(sCaminho, "[PROCESSO] - Iniciando o processo.");

            SLConnection = new SLConnection(ServiceLayerURL, Database, B1User, B1Password, 29);
            var objConfig = await SLConnection.GetAsync<List<Metadata_Config.CVA_CONFIG_MAG>>("CVA_CONFIG_MAG");
            var token = new Token();

            Util.GravarLog(sCaminho, "[PROCESSO] - Conexão HANA (OK)");

            try
            {
                token.apiAddressUri = objConfig[0].U_ApiUrl; //MagentoURL
                token.username = objConfig[0].U_ApiUsuario; //MagentoUser
                token.password = objConfig[0].U_ApiSenha; //MagentoPassword
                token.MagentoClientId = objConfig[0].U_ApiClientId; //MagentoClientId
                token.MagentoClientSecret = objConfig[0].U_ApiClientSecret; //MagentoClientSecret
                Token.create_CN(token);

                if (String.IsNullOrEmpty(token.bearerToken))
                    throw new Exception("Bearer Token não gerado.");

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão Magento (OK)");
                token.bearerToken = token.bearerToken.Replace('"', ' ').Trim();
                var oSales = new Metadata_SalesOrderStatus.Sales();

                #region [ Despachado ]

                try
                {
                    var objSales = await SLConnection.GetAsync<List<Metadata_SalesOrderStatus.Items>>("sml.svc/CVA_MAGENTO_SALESDESPACHADAS");
                    
                    var vendas = from s in objSales
                                 select new { U_CVA_Magento_Entity = s.U_CVA_Magento_Entity };

                    var vendasDistinct = vendas.Distinct();
                    SalesStatusDespachado.Item oItem = new SalesStatusDespachado.Item();
                    string sMensagemErro = String.Empty;

                    foreach (var item in vendasDistinct)
                    {
                        Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - Pedido: {0} - Status: pedido_entregue.", item.U_CVA_Magento_Entity));

                        var salesItens = objSales.Where(s => s.U_CVA_Magento_Entity == item.U_CVA_Magento_Entity);
                        int iPedidoSAP = 0;

                        oItem.items = new List<SalesStatusDespachado.Item2>();

                        foreach (var itemLines in salesItens)
                        {
                            iPedidoSAP = itemLines.Id_Pedido_SAP;                            
                            oItem.items.Add(new SalesStatusDespachado.Item2
                            {
                                order_item_id = itemLines.U_CVA_Magento_ItemId,
                                qty = itemLines.Quantity
                            });
                        }

                        SalesOrderStatusController.SalesStatusDespachadoToMagento(token, item.U_CVA_Magento_Entity, sCaminho, oItem, ref sMensagemErro);

                        DateTime horaAtual = DateTime.Now.AddSeconds(1);
                        oSales.U_CVA_Magento_Status = (String.IsNullOrEmpty(sMensagemErro) ? "2" : "1");
                        oSales.U_CVA_Magento_Data = String.Format(@"{0}-{1}-{2}", horaAtual.Year.ToString(), horaAtual.Month.ToString().PadLeft(2, '0'), horaAtual.Day.ToString().PadLeft(2, '0'));
                        oSales.U_CVA_Magento_Hora = Convert.ToInt32(String.Format(@"{0}{1}{2}", horaAtual.Hour.ToString(), horaAtual.Minute.ToString().PadLeft(2, '0'), horaAtual.Second.ToString().PadLeft(2, '0')));
                        oSales.U_CVA_Magento_Msg = sMensagemErro;

                        await SLConnection.PatchAsync($"Orders({iPedidoSAP})", oSales);
                    }   
                }
                catch (Exception ex)
                {
                    Util.GravarLog(sCaminho, String.Format(@"[ERRO] - Detalhes: {0}", ex.Message));
                }

                #endregion

                #region [ NF-e Emitida ]

                try
                {
                    var objSales = await SLConnection.GetAsync<List<Metadata_SalesOrderStatus.Items>>("sml.svc/CVA_MAGENTO_SALESNFE");
                    string sMensagemErro = String.Empty;

                    foreach (var item in objSales)
                    {
                        Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - Pedido: {0} - Status: nota_fiscal.", item.U_CVA_Magento_Entity));

                        SalesOrderStatusController.SalesStatusToMagento(token, item.U_CVA_Magento_Entity, "nota_fiscal", sCaminho, ref sMensagemErro);

                        DateTime horaAtual = DateTime.Now.AddSeconds(1);
                        oSales.U_CVA_Magento_Status = (String.IsNullOrEmpty(sMensagemErro) ? "3" : "2");
                        oSales.U_CVA_Magento_Data = String.Format(@"{0}-{1}-{2}", horaAtual.Year.ToString(), horaAtual.Month.ToString().PadLeft(2, '0'), horaAtual.Day.ToString().PadLeft(2, '0'));
                        oSales.U_CVA_Magento_Hora = Convert.ToInt32(String.Format(@"{0}{1}{2}", horaAtual.Hour.ToString(), horaAtual.Minute.ToString().PadLeft(2, '0'), horaAtual.Second.ToString().PadLeft(2, '0')));
                        oSales.U_CVA_Magento_Msg = sMensagemErro;

                        await SLConnection.PatchAsync($"Orders({item.Id_Pedido_SAP})", oSales);
                    }
                }
                catch (Exception ex)
                {
                    Util.GravarLog(sCaminho, String.Format(@"[ERRO] - Detalhes: {0}", ex.Message));
                }

                #endregion

                #region [ Pedido Cancelado ]

                try
                {
                    var objSales = await SLConnection.GetAsync<List<Metadata_SalesOrderStatus.Items>>("sml.svc/CVA_MAGENTO_SALESCANCELADAS");
                    string sMensagemErro = String.Empty;

                    foreach (var item in objSales)
                    {
                        Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - Pedido: {0} - Status: pedido_cancelado.", item.U_CVA_Magento_Entity));

                        SalesOrderStatusController.SalesStatusToMagento(token, item.U_CVA_Magento_Entity, "pedido_cancelado", sCaminho, ref sMensagemErro);

                        DateTime horaAtual = DateTime.Now.AddSeconds(1);
                        oSales.U_CVA_Magento_Status = (String.IsNullOrEmpty(sMensagemErro) ? "4" : "1");
                        oSales.U_CVA_Magento_Data = String.Format(@"{0}-{1}-{2}", horaAtual.Year.ToString(), horaAtual.Month.ToString().PadLeft(2, '0'), horaAtual.Day.ToString().PadLeft(2, '0'));
                        oSales.U_CVA_Magento_Hora = Convert.ToInt32(String.Format(@"{0}{1}{2}", horaAtual.Hour.ToString(), horaAtual.Minute.ToString().PadLeft(2, '0'), horaAtual.Second.ToString().PadLeft(2, '0')));
                        oSales.U_CVA_Magento_Msg = sMensagemErro;

                        //var jsonErro = JsonConvert.SerializeObject(oSales);
                        //Util.GravarLog(sCaminho, String.Format(@"[ERRO] - JSON: {0}", jsonErro));

                        await SLConnection.PatchAsync($"Orders({item.Id_Pedido_SAP})", oSales);

                        BusinessPartners oBP = new BusinessPartners();
                        oBP.CardFName = String.Format(@"{0} ", item.CardFName);

                        await SLConnection.PatchAsync($"BusinessPartners('{item.CardCode}')", oBP);
                    }
                }
                catch (Exception ex)
                {
                    Util.GravarLog(sCaminho, String.Format(@"[ERRO] - Detalhes: {0}", ex));
                }

                #endregion

            }
            catch (FlurlHttpException ex)
            {
                Util.GravarLog(sCaminho, String.Format(@"[ERRO] - {0}", ex.Message));
                var responseString = await ex.Call.Response.GetStringAsync();
            }
            catch (Exception ex)
            {
                Util.GravarLog(sCaminho, String.Format(@"[ERRO] - {0}", ex.Message));
            }

            Util.GravarLog(sCaminho, "[PROCESSO] - Processo finalizado.");
        }
    }
}
