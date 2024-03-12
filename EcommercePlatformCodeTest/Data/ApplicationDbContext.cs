using EcommercePlatformCodeTest.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommercePlatformCodeTest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTransaction> UserTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
    }

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Define seed data
            builder.HasData(
                new Product
                {
                    Id = 1,
                    Title = "Alaskan Malamute",
                    Description = "The Chihuahua[a] (or Spanish: Chihuahueño) is a Mexican breed of toy dog. It is named for the Mexican state of Chihuahua and is among the smallest of all dog breeds.[3] It is usually kept as a companion animal or for showing",
                    Price = 2000.00m,
                    ImageData = "dog5.jpg",
                    Stock = 6,
                    Category = "dog",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Product
                {
                    Id = 2,
                    Title = "Australian Cattle",
                    Description = "The Australian Cattle Dog, or simply Cattle Dog, is a breed of herding dog developed in Australia for droving cattle over long distances across rough terrain. This breed is a medium-sized, short-coated dog that occurs in two main colour forms. It has either red or black hair distributed fairly evenly through a white coat, which gives the appearance of a \"red\" or \"blue\" dog",
                    Price = 14000.00m,
                    ImageData = "dog2.jpg",
                    Stock = 1,
                    Category = "dog",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                 new Product
                 {
                     Id = 3,
                     Title = "Border Collie",
                     Description = "The Border Collie is a British breed of herding dog of the collie type of medium size. It originates in the region of the Anglo-Scottish border, and descends from the traditional sheepdogs once found all over the British Isles. It is kept mostly as a working sheep-herding dog or as a companion animal.[2] It competes with success in sheepdog trials. It has been claimed that it is the most intelligent breed of dog.",
                     Price = 2000.00m,
                     ImageData = "dog3.jpg",
                     Stock = 3,
                     Category = "dog",
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now
                 },
                 new Product
                 {
                     Id = 4,
                     Title = "Bulldog",
                     Description = "Bulldogs have characteristically wide heads and shoulders along with a pronounced mandibular prognathism. There are generally thick folds of skin on the brow; round, black, wide-set eyes; a short muzzle with characteristic folds called a rope or nose roll above the nose; hanging skin under the neck; drooping lips and pointed teeth, and an underbite with an upturned jaw. The coat is short, flat, and sleek with colours of red, fawn, white, brindle, and piebald.[17] They have short tails that can either hang down straight or be tucked in a coiled \"corkscrew\" into a tail pocket.",
                     Price = 1000.00m,
                     ImageData = "dog4.jpg",
                     Stock = 1,
                     Category = "dog",
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now
                 },
                 new Product
                 {
                     Id = 5,
                     Title = "Chihuahua",
                     Description = "The Chihuahua[a] (or Spanish: Chihuahueño) is a Mexican breed of toy dog. It is named for the Mexican state of Chihuahua and is among the smallest of all dog breeds.[3] It is usually kept as a companion animal or for showing",
                     Price = 2000.00m,
                     ImageData = "dog5.jpg",
                     Stock = 3,
                     Category = "dog",
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now
                 },
                 new Product
                 {
                     Id = 6,
                     Title = "Dachshund",
                     Description = "The dachshund (UK: /ˈdækshʊnd, -ənd, -hʊnt/ DAKS-huund, -⁠ənd, -⁠huunt or US: /ˈdɑːkshʊnt, -hʊnd, -ənt/ DAHKS-huunt, -⁠huund, -⁠ənt;[1][2][3][4] German: \"badger dog\"), also known as the wiener dog or sausage dog, badger dog and doxie, is a short-legged, long-bodied, hound-type dog breed. The dog may be smooth-haired, wire-haired, or long-haired. Coloration varies",
                     Price = 1000.00m,
                     ImageData = "dog6.jpg",
                     Stock = 2,
                     Category = "dog",
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now
                 },
                 new Product
                 {
                     Id = 7,
                     Title = "The American Kennel",
                     Description = "The American Kennel Club standard for the French Bulldog states that it should be muscular, with a soft and loose coat forming wrinkles.[6]\r\n\r\nThe AKC Standard weight for a French Bulldog is at maximum 28 pounds (13 kg).[16] The head of a French bulldog should be square shaped and large, with ears that resemble bat ears.[16] French bulldogs are a flat-faced breed.[17] Eyes that are AKC Standard approved for French Bulldogs are dark, almost to the point of being black; blue eyed French bulldogs are not AKC approved.[16] The coat of a French bulldog should be short haired and fine and silky.[17] Acceptable colors under the breed standard are the various shades of brindle, fawn, cream or white with brindle patches (known as \"pied\"). The fawn colors can be any light through red.[16] The most common colors are brindle, then fawn, with pieds being less common than the other colors.[18] The breed clubs do not recognize any other colors or patterns",
                     Price = 2000.00m,
                     ImageData = "dog7.jpg",
                     Stock = 4,
                     Category = "dog",
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now
                 },
                 new Product
                 {
                     Id = 8,
                     Title = "German_Shepherd",
                     Description = "German Shepherds are medium to large-sized dogs.[26] The breed standard height at the withers is 60–65 cm (24–26 in) for males, and 55–60 cm (22–24 in) for females.[27][28][29] German Shepherds can reach sprinting speeds of up to 30 miles per hour.[30] Shepherds are longer than they are tall, with an ideal proportion of 10 to 8+1⁄2. The AKC official breed standard does not set a standard weight range.[31] They have a domed forehead, a long square-cut muzzle with strong jaws and a black nose. The eyes are medium-sized and brown. The ears are large and stand erect, open at the front and parallel, but they often are pulled back during movement. A German Shepherd has a long neck, which is raised when excited and lowered when moving at a fast pace as well as stalking. The tail is bushy and reaches to the hock.[28]",
                     Price = 1000.00m,
                     ImageData = "dog8.jpg",
                     Stock = 4,
                     Category = "dog",
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now
                 },
                 new Product
                 {
                     Id = 9,
                     Title = "Golden Retriever",
                     Description = "The Golden Retriever is a powerfully built, medium-sized breed of dog; according to the Kennel Club breed standard, dogs stand from 56 to 61 centimetres (22 to 24 in) and bitches from 51 to 56 centimetres (20 to 22 in).[6][9] Healthy adult examples typically weigh between 25 and 34 kilograms (55 and 75 lb).[10]",
                     Price = 2000.00m,
                     ImageData = "dog9.jpg",
                     Stock = 1,
                     Category = "dog",
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now
                 },
                 new Product
                 {
                     Id = 10,
                     Title = "Labrador",
                     Description = "The Labrador Retriever or simply Labrador is a British breed of retriever gun dog. It was developed in the United Kingdom from St. John's water dogs imported from the colony of Newfoundland (now a province of Canada), and was named after the Labrador region of that colony. It is among the most commonly kept dogs in several countries, particularly in the European world.",
                     Price = 1500.00m,
                     ImageData = "dog10.jpg",
                     Stock = 4,
                     Category = "dog",
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now
                 },
                  new Product
                  {
                      Id = 11,
                      Title = "Border Collie",
                      Description = "The Border Collie is a British breed of herding dog of the collie type of medium size. It originates in the region of the Anglo-Scottish border, and descends from the traditional sheepdogs once found all over the British Isles. It is kept mostly as a working sheep-herding dog or as a companion animal.[2] It competes with success in sheepdog trials. It has been claimed that it is the most intelligent breed of dog.",
                      Price = 1000.00m,
                      ImageData = "dog11.jpg",
                      Stock = 4,
                      Category = "dog",
                      CreatedAt = DateTime.Now,
                      UpdatedAt = DateTime.Now
                  },
                  new Product
                  {
                      Id = 12,
                      Title = "Parrots",
                      Description = "Parrots, also known as psittacines, are birds with a strong curved beak, upright stance, and clawed feet. They are conformed by four families that contain roughly 410 species in 101 genera, found mostly in tropical and subtropical regions.",
                      Price = 1000.00m,
                      ImageData = "bird1.jpg",
                      Stock = 3,
                      Category = "bird",
                      CreatedAt = DateTime.Now,
                      UpdatedAt = DateTime.Now
                  },
                  new Product
                  {
                      Id = 13,
                      Title = "Owl",
                      Description = "Owls are birds from the order Strigiformes, which includes over 200 species of mostly solitary and nocturnal birds of prey typified by an upright stance, a large, broad head, binocular vision, binaural hearing, sharp talons, and feathers adapted for silent flight.",
                      Price = 900.00m,
                      ImageData = "bird2.jpg",
                      Stock = 3,
                      Category = "bird",
                      CreatedAt = DateTime.Now,
                      UpdatedAt = DateTime.Now
                  },
                  new Product
                  {
                      Id = 14,
                      Title = "Columbidae",
                      Description = "Columbidae is a bird family consisting of doves and pigeons. It is the only family in the order Columbiformes.",
                      Price = 1000.00m,
                      ImageData = "bird5.jpg",
                      Stock = 5,
                      Category = "bird",
                      CreatedAt = DateTime.Now,
                      UpdatedAt = DateTime.Now
                  },
                    new Product
                    {
                        Id = 15,
                        Title = "Toucan",
                        Description = "Toucans are Neotropical members of the near passerine bird family Ramphastidae. The Ramphastidae are most closely related to the American barbets.",
                        Price = 8000.00m,
                        ImageData = "bird4.jpg",
                        Stock = 3,
                        Category = "bird",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                      new Product
                      {
                          Id = 16,
                          Title = "Indian peafowl",
                          Description = "The Indian peafowl (Pavo cristatus), also known as the common peafowl, and blue peafowl, is a peafowl species native to the Indian subcontinent.",
                          Price = 3000.00m,
                          ImageData = "bird6.jpg",
                          Stock = 1,
                          Category = "bird",
                          CreatedAt = DateTime.Now,
                          UpdatedAt = DateTime.Now
                      },
                        new Product
                        {
                            Id = 17,
                            Title = "British Shorthair",
                            Description = "The British Shorthair is the pedigreed version of the traditional British domestic cat, with a distinctively stocky body, thick coat, and broad face.",
                            Price = 1000.00m,
                            ImageData = "cat1.jpg",
                            Stock = 1,
                            Category = "cat",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        },
                      new Product
                      {
                          Id = 18,
                          Title = "Persian cat",
                          Description = "The Persian cat, also known as the Persian Longhair, is a long-haired breed of cat characterised by a round face and short muzzle.",
                          Price = 7000.00m,
                          ImageData = "cat2.jpg",
                          Stock = 4,
                          Category = "cat",
                          CreatedAt = DateTime.Now,
                          UpdatedAt = DateTime.Now
                      },
                         new Product
                         {
                             Id = 19,
                             Title = "American Shorthair",
                             Description = "The American Shorthair is a medium to large sized cat breed with males weighing between 11-15 lbs (5-7 kg) and females weighing between 6-12 lbs (2.75-5.5kg).",
                             Price = 8000.00m,
                             ImageData = "cat3.jpg",
                             Stock = 3,
                             Category = "cat",
                             CreatedAt = DateTime.Now,
                             UpdatedAt = DateTime.Now
                         },
                            new Product
                            {
                                Id = 20,
                                Title = "Turkish Van",
                                Description = "The Turkish Van is a large, muscular cat with a moderately long body and tail. It has strong, broad shoulders and a short neck; the jock of the cat world.",
                                Price = 3000.00m,
                                ImageData = "cat5.jpg",
                                Stock = 4,
                                Category = "cat",
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            }

            );
        }
    }
}
