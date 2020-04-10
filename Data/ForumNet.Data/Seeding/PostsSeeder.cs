namespace ForumNet.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using ForumNet.Common;
    using Models;
    using Models.Enums;

    internal class PostsSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Posts.AnyAsync())
            {
                return;
            }

            var adminId = await dbContext.Users
                .Where(u => u.UserName == GlobalConstants.AdministratorUserName)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            var testUserId = await dbContext.Users
                .Where(u => u.UserName == GlobalConstants.TestUserUserName)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            var categoryId = await dbContext.Categories
                .Where(c => c.Name == "Programming")
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            var tagIds = await dbContext.Tags
                .Where(t => t.Name == "C#" || t.Name == "MongoDB" || t.Name == "NoSQL")
                .Select(t => t.Id)
                .ToListAsync();

            var posts = new List<Post>
            {
                new Post
                {
                    Title = @"Какво наричаме ""NoSQL бази данни""?",
                    Description = @"<div class=""news-img""><img title=""Профил"" src=""https://softuni.bg/Files/Publications/2020/02/monodb-blog_02643555.png"" alt=""Какво наричаме &quot;NoSQL бази данни&quot;?"" /></div>
    <div class=""html-raw-wrapper"">
    <p style=""text-align: justify;""> Да усвоите изграждането на back-end логиката е само една част от изграждането на пълноценно уеб приложение.Все пак в крайна сметка с него ще боравят потребители, които ще предоставят някаква информация за себе си и нуждите си, която е добре да съхранявате.За да съхраните тази информация вие имате нужда от база данни.</p>
    <h2 style=""text-align: justify;""> Какво е ""база данни""?</h2>
    <p style=""text-align: justify;""> Най - общо казано база данни е организирана колекция от данни, която се съхранява и достъпва електронно, чрез компютърна система.Базите данни могат да се различават по редица фактори, като начина на съхранение на данни, структурата, която използват за целта и много други.Базата данни се достъпва и манипулира посредством система за управление на бази данни (<em>или както ще ги срещнете по-често ""Database Management System"" или ""DBMS""</em>). Тя представлява софтуер, който взаимодейства с крайния потребител, приложението и самата база данни, улавяйки и анализирайки данните.DBMS софтуерът допълнително обхваща и ключови средства, които се използват за администриране на базата данни. Общия сбор на базата данни, системата за управление на база данни и свързаните с тях приложения, не е грешно да се нарекат ""Система от бази данни"".<br /><br />Както вече посочихме базите данни могат да бъдат множество типове, като например йерархични, мрежови, релационни, нерелационни и базирани на още много други модели за бази данни.Едни от най-разпространените типове бази данни са релационните, които доминират през 80-те и 90-те години. В началото на XXI-ви век обаче постепенно започват да се налагат нерелационните, или както са по-известни днес като ""NoSQL"".</p>
    <h2 style=""text-align: justify;""> Какво е ""NoSQL"" база данни?</h2>
    <p style=""text-align: justify;""> NoSQL бази данни (<em>известни и като ""non SQL"" или ""нерелационни""</em>) осигуряват механизъм за съхранение и обработка на данни, който е моделиран по начин, различен от табличните релации(взаимовръзки), които са използвани при релационните бази данни.Този модел бази данни съществува от края на 60-те, но името ""NoSQL"" се превръща в нарицателно за него едва в началото на този век, най-вече поради нуждите на т.нар. ""Web 2.0"" компании.Към наши дни NoSQL базите данни намират все по-широко приложение в сфери като ""Big Data"" и реалновремеви уеб приложения.Понякога NoSQL системите са наричани и ""Not only SQL"", за да се подчертае възможността им да поддържат SQL-подобни езици за заявки, или да се използват заедно със SQL бази данни, в рамките на полиглотично-стабилни системи (<em>системи, които са проектирани да съвместяват няколко модела на бази данни, с цел да обслужват различни нужди при обработката на данните</em>).<br /><br />Предимствата на NoSQL модела включват: опростен дизайн, по-просто ""хоризонтално"" скалиране при компютърни клъстъри(което е проблем за релационните бази данни) и много други.Структурите от данни, които използват NoSQL базите данни(например key-value, wide column, document или graph) са различни от тези, които се използват по подразбиране при релационните бази данни, което прави извършването на някои операции по-бързо, а в определени случаи - и по-гъвкаво, спрямо релационните бази данни.</p>
    <h2 style=""text-align: justify;""> Към изучаването на каква база данни да се ориентирате?</h2>
    <p style=""text-align: justify;""> Ако сте се ориентирали в посока на разработка на уеб приложения - то NoSQL базите данни са едно логично решение. Ако имате изградени всички фундаментални знания при работа с JavaScript, познавате синтаксиса, работите без проблеми с функции, масиви, обекти и JSON документи - то тогава един добър избор за развитие на вашите умения е NoSQL базата данни&nbsp;<strong>MongoDB</strong>&nbsp;- кросплатформена, документно-ориентирана система за бази данни, която борави с JSON-подобни документи.</p>
    </div>",
                    Type = PostType.Discussion,
                    AuthorId = adminId,
                    CategoryId = categoryId,
                    IsPinned = true,
                    CreatedOn = DateTime.UtcNow,
                    Tags = new List<PostTag>
                    {
                        new PostTag
                        {
                            TagId = tagIds[1]
                        },
                        new PostTag
                        {
                            TagId = tagIds[2]
                        },
                    }
                },
                new Post
                {
                    Title = "3 полезни факта за C#, които ще ви е интересно да научите",
                    Description = @"<div class=""news-img"" style=""text-align: justify;""><img title=""Софтуерно Инженерство"" src=""https://softuni.bg/Files/Publications/2020/03/93b75264fc117c6de7e6623f9adf48cbea7cf399_9498942.png"" alt=""3 полезни факта за C#, които ще ви е интересно да научите"" /></div>
<div class=""html-raw-wrapper"">
<p style=""text-align: justify;"">Програмният език&nbsp;<strong>C#</strong>&nbsp;е сочен от специалистите за един от най-подходящите, с които да се стартира обучение по софтуерно инженерство. Той е широко използван, пише се сравнително лесно и могат да се намерят множество ресурси, свързани с него. В следващите редове ще ви запознаем с&nbsp;<strong>3 полезни факта</strong>, съпътстващи този съвременен език за програмиране. Ако се вдъхновите да го опознаете още по-отблизо,&nbsp;<strong>обучението</strong>&nbsp;на инициативата ни Софтуерен университет -&nbsp;<strong><a href=""https://softuni.bg/trainings/2896/programming-basics-with-c-sharp-april-2020"">Programming basics with C#</a></strong>&nbsp;- е прекрасна възможност да положите основи в една от най-перспективните сфери в света.</p>
<hr />
<h2 style=""text-align: justify;"">1. Знаете ли, че за кратко време C# се е казвал Cool?</h2>
<p style=""text-align: justify;"">В зората на създаването си, програмният език C# се е наричал&nbsp;<strong>Cool</strong>. През януари 1999 г. Microsoft привлича Андерс Хейлсбърг, бившият главен архитект на Borland Delphi. Той оглавява екип, чиято задача е да разработи нов програмен език. Последният получава името Cool, което е абревиатура от &bdquo;<strong>C-like Object Orientated Language</strong>&ldquo;. Вероятно езикът и до днес щеше да продължи да се нарича така, ако не е съществувал рискът от засягане на нечии авторски права.</p>
<hr />
<h2 style=""text-align: justify;"">2. C# първоначално е било името на модифицирана версия на езика C&nbsp;</h2>
<p style=""text-align: justify;"">През 80-те години на миналия век, Microsoft започва да използва&nbsp;<strong>модифицирана</strong>&nbsp;<strong>версия</strong>&nbsp;на езика C, на която дава името&nbsp;<strong>C#</strong>. Този проект обаче така и не се завършва, след като работата по него бива прекратена. От него е останало само името, което напомня за себе си в точния момент, когато трябва да се преименува новосъздадения тогава Cool.</p>
<p style=""text-align: justify;"">През юни на 2000 г., по време на Professional Developers Conference, се случва официалното обявяване на този&nbsp;<strong>.NET проект с името C#</strong>, а всички ASP .NET библиотеки били портнати към него.</p>
<hr />
<h2 style=""text-align: justify;"">3. C# ще ви помогне да изпънявате различен тип задачи</h2>
<p style=""text-align: justify;"">Предназначението на всеки език, с който се пишат програми, е същите да разрешат дадени проблеми или задачи. По-конкретно със C# ще можете да правите:</p>
<p style=""text-align: justify;"">✔&nbsp;<strong>Уеб разработка</strong>. Тя е възможна с помощта на технологичната рамка NET и позволява изграждането на днамични уеб страници, посредством C# езика, като е подходяща за сървърни приложения. В случая се има предвид back-end логиката, а не дизайна на даден сайт или приложение.</p>
<p style=""text-align: justify;"">✔&nbsp;<strong>Мобилна разработка</strong>. От 2016 г. Microsoft придобива компанията Xamarin и оттогвава активно развиват C# технологията за крос-платформени приложения. С нейна помощ могат да се пишат приложения на C#, като те могат да се отворят и на macOS.</p>
<p style=""text-align: justify;"">✔&nbsp;<strong>Десктоп приложения</strong>. Предвид водещите позиции на Microsoft в света на операцонните системи, съвсем логично разработеният също от тях C# е перфектен за разписване на десктоп приложения.</p>
<p style=""text-align: justify;"">✔&nbsp;<strong>Разработване на игри</strong>. Вериятно ще ви е интересно да научите, че най-разпространеният open source game engine е написан именно на C#.</p>
</div>",
                    Type = PostType.Discussion,
                    AuthorId = adminId,
                    CategoryId = categoryId,
                    IsPinned = true,
                    CreatedOn = DateTime.UtcNow,
                    Tags = new List<PostTag>
                    {
                        new PostTag
                        {
                            TagId = tagIds[0]
                        }
                    }
                },
                new Post
                {
                    Title = "What is ML.NET",
                    Description = @"
<p style=""text-align: justify;""><strong>ML.NET</strong>&nbsp;is a&nbsp;<a title=""Free software"" href=""https://en.wikipedia.org/wiki/Free_software"">free software</a>&nbsp;<a title=""Machine learning"" href=""https://en.wikipedia.org/wiki/Machine_learning"">machine learning</a>&nbsp;<a title=""Library (computing)"" href=""https://en.wikipedia.org/wiki/Library_(computing)"">library</a>&nbsp;for the&nbsp;<a title=""C Sharp (programming language)"" href=""https://en.wikipedia.org/wiki/C_Sharp_(programming_language)"">C#</a>&nbsp;and&nbsp;<a title=""F Sharp (programming language)"" href=""https://en.wikipedia.org/wiki/F_Sharp_(programming_language)"">F#</a> programming languages.&nbsp;<sup id=""cite_ref-visu_Open_4-0"" class=""reference""></sup>It also supports&nbsp;<a title=""Python (programming language)"" href=""https://en.wikipedia.org/wiki/Python_(programming_language)"">Python</a>&nbsp;models when used together with NimbusML. The preview release of ML.NET included transforms for&nbsp;<a title=""Feature engineering"" href=""https://en.wikipedia.org/wiki/Feature_engineering"">feature engineering</a>&nbsp;like&nbsp;<a title=""N-gram"" href=""https://en.wikipedia.org/wiki/N-gram"">n-gram</a> creation, and learners to handle binary classification, multi-class classification, and regression tasks.&nbsp;<sup id=""cite_ref-gith_dotn_7-0"" class=""reference""></sup>Additional ML tasks like anomaly detection and recommendation systems have since been added, and other approaches like deep learning will be included in future versions.</p>
<hr />
<h2 style=""text-align: justify;""><span id=""Machine_learning"" class=""mw-headline"">Machine learning</span></h2>
<p style=""text-align: justify;"">ML.NET brings model-based Machine Learning analytic and prediction capabilities to existing .NET developers. The framework is built upon .NET Core and .NET Standard inheriting the ability to run cross-platform on&nbsp;<a title=""Linux"" href=""https://en.wikipedia.org/wiki/Linux"">Linux</a>,&nbsp;<a class=""mw-redirect"" title=""Windows"" href=""https://en.wikipedia.org/wiki/Windows"">Windows</a>&nbsp;and&nbsp;<a title=""MacOS"" href=""https://en.wikipedia.org/wiki/MacOS"">macOS</a>. Although the ML.NET framework is new, its origins began in 2002 as a Microsoft Research project named TMSN (text mining search and navigation) for use internally within Microsoft products. It was later renamed to TLC (the learning code) around 2011. ML.NET was derived from the TLC library and has largely surpassed its parent says Dr. James McCaffrey, Microsoft Research.</p>
<p style=""text-align: justify;"">Developers can train a Machine Learning Model or reuse an existing Model by a 3rd party and run it on any environment offline. This means developers do not need to have a background in Data Science to use the framework. Support for the&nbsp;<a title=""Open-source software"" href=""https://en.wikipedia.org/wiki/Open-source_software"">open-source</a>&nbsp;Open Neural Network Exchange (<a class=""mw-redirect"" title=""Onnx"" href=""https://en.wikipedia.org/wiki/Onnx"">ONNX</a>)&nbsp;<a class=""mw-redirect"" title=""Deep Learning"" href=""https://en.wikipedia.org/wiki/Deep_Learning"">Deep Learning</a>&nbsp;model format was introduced from build 0.3 in ML.NET. The release included other notable enhancements such as Factorization Machines, LightGBM, Ensembles, LightLDA transform and OVA.<span style=""font-size: 13.3333px;""> </span>The ML.NET integration of&nbsp;<a title=""TensorFlow"" href=""https://en.wikipedia.org/wiki/TensorFlow"">TensorFlow</a>&nbsp;is enabled from the 0.5 release. Support for x86 &amp; x64 applications was added to build 0.7 including enhanced recommendation capabilities with Matrix Factorization.<span style=""font-size: 13.3333px;""> </span>A full roadmap of planned features have been made available on the official GitHub repo.</p>",
                    Type = PostType.Discussion,
                    AuthorId = testUserId,
                    CategoryId = categoryId,
                    CreatedOn = DateTime.UtcNow,
                    Tags = new List<PostTag>
                    {
                        new PostTag
                        {
                            TagId = tagIds[0]
                        }
                    }
                }
            };

            await dbContext.Posts.AddRangeAsync(posts);
        }
    }
}
