using System;
using Npgsql;
using UnityEngine;
public static class DB
{
    public static string connectionString = string.Format("Port={0};Server={1};Database={2};User ID={3};Password={4};" +
        "sslmode=Require;Trust Server Certificate=true",
        "5432",
        "ec2-52-31-219-113.eu-west-1.compute.amazonaws.com",
        "d2k1albim4eke9",
        "ejoakkofrykkqe",
        "59816242e3c8ae5a1c0930e88c891da5f542190249021bbf500c516c08042b8e");
}