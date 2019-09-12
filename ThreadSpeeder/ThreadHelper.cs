using System;
using System.Collections.Generic;
using System.Threading;
namespace ThreadSpeeder
{
    public class ThreadHelper<TList, TObject, TListReturn, TObjectReturn> where TList : List<TObject> where TObject : class where TListReturn : List<TObjectReturn>
    {

        public void PreProcessar(TList objectList, TListReturn listRetorno, short numberOfSimultaneosThread, Func<TObject, TObjectReturn> methodToBeCalled)
        {
            if (objectList.Count > 0)
            {
                int countOfItemsToBeProcessed = 0;
                int indiceProcessar = 0;

                for (int i = 0; i < objectList.Count; i++)
                {
                    countOfItemsToBeProcessed = countOfItemsToBeProcessed + 1;
                    if (countOfItemsToBeProcessed == numberOfSimultaneosThread)
                    {
                        TList listOfObjectsToBeProcessedByRange = (TList)objectList.GetRange(indiceProcessar, numberOfSimultaneosThread);
                        Processar(listOfObjectsToBeProcessedByRange, listRetorno, methodToBeCalled);
                        countOfItemsToBeProcessed = 0;
                        indiceProcessar += numberOfSimultaneosThread;
                    }
                }
                if (countOfItemsToBeProcessed > 0)
                {
                    var ultimosObjetosAindaNaoProcessados = (TList)objectList.GetRange(objectList.Count - countOfItemsToBeProcessed, objectList.Count - indiceProcessar);
                    Processar(ultimosObjetosAindaNaoProcessados, listRetorno, methodToBeCalled);
                }
            }
        }
        private TListReturn Processar(TList objectList, TListReturn listRetorno, Func<TObject, TObjectReturn> methodToBeCalled)
        {
            try
            {
                using (var countdownEvent = new CountdownEvent(objectList.Count))
                {
                    foreach (var objeto in objectList)
                    {
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            try
                            {
                                //Faz o processamento
                                listRetorno.Add(methodToBeCalled(objeto));
                            }
                            finally
                            {
                                countdownEvent.Signal();
                            }
                        },
                        objectList.IndexOf(objeto)
                        );
                    }
                    countdownEvent.Wait();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listRetorno;
        }
    }
}
