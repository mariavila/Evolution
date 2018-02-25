using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerSim : MonoBehaviour
{

    public GameObject boomerPrefab;
    public GameObject elephantPrefab;
    public Transform foodPrefab;
    //public GameObject foodPrefab2;
    //public GameObject foodPrefab3;
    public Text generacio;

    private bool isTraning = false;
    private int populationSize = 50;
    private int elephantpopulationSize = 5;
    private int generationNumber = 0;
    private int[] layers = new int[] { 6, 5, 4, 2 }; //1 input and 1 output
    private int[] bunnylayers = new int[] { 8, 6, 4, 2 };
    private List<NeuralNetwork> nets;
    private List<NeuralNetwork> elephantnets;
    //private bool leftMouseDown = false;
    private List<BunnyMovement> boomerangList = null;
    private List<Elephant> elephantList = null;
    private Transform[] foodVector;
    private Transform[] rabbitVector;
    private Transform[] elephantVector;
    private bool pause = false;
    float veltemps;


    void Timer() //Quan isTraining està a fals, el seguent Update causara que es faci reset a la poblacio
    {
        isTraning = false;
    }


    void Update()
    {
        if (isTraning == false) //No estem entrenant -> Hora de crear nova gen
        {
            if (generationNumber == 0) //Primera gen, creada random
            {
                InitFoodVector();
                InitBoomerangNeuralNetworks();
                InitElephantNeuralNetworks();
                veltemps = Time.timeScale;
            }
            else
            {
                int selectedunits = 20;
                nets.Sort(); //Ordena ascendentment les neural nets. Duplica les 25 millors i despres les muta, per un total de 50. Les 25 pitjors moren
                for (int i = 0; i < populationSize - selectedunits; i++)
                {
                    nets[i] = new NeuralNetwork(nets[populationSize-1 - i % selectedunits]);
                    nets[i].Mutate();

                    //nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                }

                for (int i = 1; i <= selectedunits; i++)
                {
                    nets[populationSize - i] = new NeuralNetwork(nets[populationSize - i]);
                }

                for (int i = 0; i < populationSize; i++) //Reset a la fitness de tots
                {
                    nets[i].SetFitness(0f);
                }

                // Elephant turn!!!


                int selectedelephants = 2;
                elephantnets.Sort(); //Ordena ascendentment les neural nets. Duplica les 25 millors i despres les muta, per un total de 50. Les 25 pitjors moren
                for (int i = 0; i < elephantpopulationSize - selectedelephants; i++)
                {
                    elephantnets[i] = new NeuralNetwork(elephantnets[elephantpopulationSize - 1 - i % selectedelephants]);
                    elephantnets[i].Mutate();

                    //nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                }

                for (int i = 1; i <= selectedelephants; i++)
                {
                    elephantnets[elephantpopulationSize - i] = new NeuralNetwork(elephantnets[elephantpopulationSize - i]);
                }

                for (int i = 0; i < elephantpopulationSize; i++) //Reset a la fitness de tots
                {
                    elephantnets[i].SetFitness(0f);
                }
            }


            generationNumber++; //Ha passat una gen mes
            generacio.text = "Generation: " + generationNumber.ToString();
            isTraning = true; //Es posa a entrenar
            Invoke("Timer", 20f);  //Al cap de 15s, es fa la rutina "Timer" -> Cada 15 segons nova societat
            CreateBoomerangBodies(); //Spawn els boomerangs again!!!
            CreateElephantBodies();
            InitRabbitVector();
            InitElephantVector();
            LinkVectors();
        }
        
        //Control de l'hexagon. Si estas clicant, l'hexagon es mou a on estas apretant.
        if (Input.GetKeyDown("q"))
        {
            Time.timeScale *= 10f;
            veltemps = Time.timeScale;
        }
        else if (Input.GetKeyDown("e"))
        {
            Time.timeScale /= 10f;
            veltemps = Time.timeScale;
        }

        if (Input.GetKeyDown("space"))
        {
            if(pause == false)
            {
                pause = true;
                Time.timeScale = 0f;
            }
            else
            {
                pause = false;
                Time.timeScale = veltemps;
            }
        }
        
    }


    private void CreateBoomerangBodies()
    {
        if (boomerangList != null)
        {
            for (int i = 0; i < boomerangList.Count; i++)
            {
                GameObject.Destroy(boomerangList[i].gameObject); //Es carrega tots els boomerangs existents
            }

        }

        boomerangList = new List<BunnyMovement>();
        //Itera per crear tots els nous boo merangs
        for (int i = 0; i < populationSize; i++)
        {
            BunnyMovement boomer = ((GameObject)Instantiate(boomerPrefab, new Vector3(UnityEngine.Random.Range(-40f,40f), 0, UnityEngine.Random.Range(-40f,40f)), boomerPrefab.transform.rotation)).GetComponent<BunnyMovement>(); //Crea un boomerang i en pilla el punter a la mateix linia
            //boomer.Init(nets[i], foodVector, elephantVector); //Valors inicials del boomerang, com la neural net que usa o on esta l'hexagon
            boomerangList.Add(boomer);
        }

    }

    private void CreateElephantBodies()
    {
        if (elephantList != null)
        {
            for (int i = 0; i < elephantList.Count; i++)
            {
                GameObject.Destroy(elephantList[i].gameObject); //Es carrega tots els boomerangs existents
            }

        }

        elephantList = new List<Elephant>();
        //Itera per crear tots els nous boo merangs
        for (int i = 0; i < elephantpopulationSize; i++)
        {
            Elephant boomer = ((GameObject)Instantiate(elephantPrefab, new Vector3(UnityEngine.Random.Range(-60f, 60f), 0, UnityEngine.Random.Range(-40f, 40f)), elephantPrefab.transform.rotation)).GetComponent<Elephant>(); //Crea un boomerang i en pilla el punter a la mateix linia
            //boomer.Init(elephantnets[i], rabbitVector); //Valors inicials del boomerang, com la neural net que usa o on esta l'hexagon
            elephantList.Add(boomer);
        }

    }

    void LinkVectors()
    {
        int i = 0;
        foreach(Elephant aux in elephantList)
        {
            aux.Init(elephantnets[i], rabbitVector);
            i++;
        }

        i = 0;
        foreach (BunnyMovement aux in boomerangList)
        {
            aux.Init(nets[i], foodVector, elephantVector);
            i++;
        }
    }

    void InitFoodVector()
    {
        int amountfood = 100;
        foodVector = new Transform[amountfood];
        for(int i = 0; i < amountfood; i++)
        {
            foodVector[i] = Instantiate(foodPrefab, new Vector3(UnityEngine.Random.Range(-40f, 40f), 0.5f, UnityEngine.Random.Range(-40f, 40f)), Quaternion.identity);
        }
    }

    void InitRabbitVector()
    {
        rabbitVector = new Transform[boomerangList.Count];
        int i = 0;
        foreach(BunnyMovement bunny in boomerangList)
        {
            rabbitVector[i] = bunny.transform;
            i++;
        }
    }

    void InitElephantVector()
    {
        elephantVector = new Transform[elephantList.Count];
        int i = 0;
        foreach (Elephant bunny in elephantList)
        {
            elephantVector[i] = bunny.transform;
            i++;
        }
    }

    void InitBoomerangNeuralNetworks() //Crea una poblacio de zero amb individus aleatoris
    {
        //population must be even, just setting it to 20 incase it's not
        if (populationSize % 2 != 0)
        {
            populationSize = 20;
        }

        nets = new List<NeuralNetwork>();


        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(bunnylayers); //Nova neural net amb la distrib escollida
            net.Mutate(); //Es muta
            nets.Add(net); //S'afegeix a la llista
        }
    }

    void InitElephantNeuralNetworks() //Crea una poblacio de zero amb individus aleatoris
    {
        //population must be even, just setting it to 20 incase it's not
 

        elephantnets = new List<NeuralNetwork>();


        for (int i = 0; i < elephantpopulationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers); //Nova neural net amb la distrib escollida
            net.Mutate(); //Es muta
            elephantnets.Add(net); //S'afegeix a la llista
        }
    }
}
