using FeedbackAnalyzer.Domain.Entities;
using FeedbackAnalyzer.Infrastructure.Repositories;

namespace FeedbackAnalyzer.Infrastructure.Data;

public static class SeedData
{
    public static void Initialize(InMemoryBusinessRepository repo)
    {
        var businesses = new[]
        {
            Business.Create("Pizzeria Bella Roma", "Restaurant", "Cluj-Napoca",
                "Pizzerie autentica italiana in centrul Clujului. Ingrediente proaspete, cuptor cu lemne."),

            Business.Create("Auto Service Mitica", "Auto Service", "Bucuresti",
                "Service auto cu experienta de peste 15 ani. Reparatii rapide si preturi corecte."),

            Business.Create("Salon Elegance", "Salon de Frumusete", "Timisoara",
                "Salon modern cu servicii complete: tuns, vopsit, tratamente."),

            Business.Create("Libraria Carturar Local", "Librarie", "Iasi",
                "Librarie independenta cu o selectie atenta de carte romaneasca si internationala."),

            Business.Create("Gym Power Zone", "Fitness", "Brasov",
                "Sala de fitness cu echipamente moderne, antrenori certificati si clase de grup."),
        };

        repo.Seed(businesses);
    }
}
