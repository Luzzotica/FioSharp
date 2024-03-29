﻿using FioSharp.Core;
using FioSharp.Core.Providers;
using FioSharp.Unity3D;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FioSharp.UnitTests.Unity3D
{
    public class StressUnitTests
    {
        Fio Eos { get; set; }
        public StressUnitTests()
        {
            Eos = new Fio(new FioConfigurator()
            {
                SignProvider = new DefaultSignProvider("5K57oSZLpfzePvQNpsLS6NfKXLhhRARNU13q6u2ZPQCGHgKLbTA"),

                HttpEndpoint = "https://api.eossweden.se", //Mainnet
                ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"

                //HttpEndpoint = "https://nodeos01.btuga.io",
                //ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f"
            });
        }

        public async Task GetBlockTaskLoop()
        {
            bool success = false;
            int nrTasks = 50;
            int nrBlocks = 1000;
            int blockStartPos = 100;
            int taskBlocks = nrBlocks / nrTasks;

            try
            {
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < nrTasks; i++)
                {
                    for (int j = 1; j <= taskBlocks; j++)
                    {
                        try
                        {
                            await Eos.GetBlock((i * taskBlocks + blockStartPos + j).ToString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(JsonConvert.SerializeObject(ex));
                        }
                    }
                }

                await Task.WhenAll(tasks.ToArray());

                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            if (success)
                Console.WriteLine("Test GetBlockTaskLoop run successfuly.");
            else
                Console.WriteLine("Test GetBlockTaskLoop run failed.");
        }

        public async Task TestAll()
        {
            //TODO disabled for now because of CORS policy blocked in localhost
            //await GetBlockTaskLoop();
            await Task.FromResult(0);
        }
    }
}
