﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
            "Values('Coca-Cola Diet', 'Refrigerante de Cola 350 ml', 5.45, 'cocacola.jpg', 50, getdate(), 1)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
            "Values('Lanche de Atum', 'RLanche de Atum com maionese', 8.50, 'atum.jpg', 10, getdate(), 2)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
            "Values('Pudim 100 g', 'Pudim de leite condensado 100g', 6.75, 'pudim.jpg', 20, getdate(), 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Produtos");
        }
    }
}
