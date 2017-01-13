﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace Bank
{
    public class Data
    {
        //Ajouter la référence Configuration !
        static string sqlCon = ConfigurationManager.ConnectionStrings["BankConnectionString"].ConnectionString;

        public static void Insert(Extrait extrait)
        {
            InsertBeneficiaire(extrait.Beneficiaire);
            InsertExtrait(extrait);
        }
        private static void InsertBeneficiaire(Beneficiaire beneficiaire)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlCon))
                {
                    
                    try
                    {
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        
                        cmd.Parameters.AddWithValue("nom", beneficiaire.Nom);
                        cmd.Parameters.AddWithValue("compte", beneficiaire.Compte ?? Convert.DBNull);
                        cmd.Parameters.AddWithValue("adresse", beneficiaire.Adresse ?? Convert.DBNull);

                        cmd.CommandText = "SELECT COUNT(*) FROM Beneficiaire WHERE compte=@compte AND nom=@nom";
                        if ((int)cmd.ExecuteScalar() == 0)
                        {
                            cmd.CommandText = "INSERT INTO Beneficiaire(nom,compte,adresse)" +
                                          "VALUES(@nom, @compte,@adresse)";

                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            Console.WriteLine("Ce bénéficiaire existe déjà");
                        }
                        cmd.CommandText = "SELECT id FROM Beneficiaire WHERE compte=@compte AND nom=@nom"; //Que se passe t-il si compte est NULL ??? => insertion multiple !
                        
                        beneficiaire.Id = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void InsertExtrait(Extrait extrait)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlCon))
                {
                    try  
                    {
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();

                        cmd.Parameters.AddWithValue("numSequence", extrait.Sequence);
                        cmd.Parameters.AddWithValue("anneeSequence", extrait.DateExecution.Year);
                        cmd.Parameters.AddWithValue("montant", extrait.Montant);
                        cmd.Parameters.AddWithValue("devise", extrait.Devise);
                        cmd.Parameters.AddWithValue("avecCarte", extrait.AvecCarte);
                        cmd.Parameters.AddWithValue("dateValeur", extrait.DateValeur);
                        cmd.Parameters.AddWithValue("dateExecution", extrait.DateExecution);
                        cmd.Parameters.AddWithValue("id_beneficiaire", extrait.Beneficiaire.Id);
                        cmd.Parameters.AddWithValue("communication", extrait.Communication ?? Convert.DBNull);
                        cmd.Parameters.AddWithValue("mandat", extrait.Mandat ?? Convert.DBNull);
                        cmd.Parameters.AddWithValue("reference", extrait.Reference ?? Convert.DBNull);
                        cmd.Parameters.AddWithValue("details", extrait.Details ?? Convert.DBNull);

                        cmd.CommandText = "SELECT COUNT(*) FROM Extrait WHERE NumSequence=@numSequence AND anneeSequence=@anneeSequence";
                        if ((int)cmd.ExecuteScalar()==0)
                        {
                            cmd.CommandText = "INSERT INTO Extrait(numSequence, anneeSequence,montant,devise, avecCarte,dateValeur,dateExecution,communication,mandat,reference,details,id_beneficiaire)" +
                                "VALUES(@numSequence,@anneeSequence,@montant,@devise,@avecCarte,@dateValeur,@dateExecution,@communication,@mandat,@reference,@details,@id_beneficiaire)";
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            Console.WriteLine("L'extrait a déjà été ajouté");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

