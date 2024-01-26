using Common.AutoMappers;
using Common.ExceptionFilters;
using Common.Filter.UnitOfWork;
using Common.Helper;
using Common.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text;
using Zack.Commons;

namespace Best_WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddNewtonsoftJson(option =>
                {
                    //��Сд����
                    option.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //ͳһʱ���ʽ����
                    option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //token��֤
            builder.Services.AddSwaggerGen(c =>
            {
                var scheme = new OpenApiSecurityScheme()
                {
                    Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    },
                    Scheme = "oauth2",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                };
                c.AddSecurityDefinition("Authorization", scheme);
                var requirement = new OpenApiSecurityRequirement();
                requirement[scheme] = new List<string>();
                c.AddSecurityRequirement(requirement);
            });

            //Jwt��������
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    var jwtOpt = builder.Configuration.GetSection("JWT").Get<JwtOptions>();
                    byte[] secBytes = Encoding.UTF8.GetBytes(jwtOpt.SecKey);
                    var seckey = new SymmetricSecurityKey(secBytes);
                    opt.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = seckey,
                        ValidIssuer = jwtOpt.Issuer,//�����ڼ�����Ƶķ������Ƿ���˷�������ͬ
                        ValidAudience = jwtOpt.Audience,//������Ƶ�����Ⱥ���Ƿ��������Ⱥ����ͬ
                    };
                });

            //�������ݿ�
            var aseembly = ReflectionHelper.GetAllReferencedAssemblies();
            builder.Services.AddAllDbContexts(opt =>
            {
                DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();
                var connectStr = CommonHelper.GetConnecttionStr();
                var serverVersion=ServerVersion.AutoDetect(connectStr);
                dbContextOptionsBuilder.UseMySql(connectStr,serverVersion);
            },aseembly);

            //������
            builder.Services.Configure<MvcOptions>(opt =>
            {
                opt.Filters.Add<exceptionFilter>();
                opt.Filters.Add<UnitOfWorkFilter>();
            });

            //�������⣨��ʽ����ͨ��nginxת����
            string[] orgins = new string[] { };
            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(buider=> buider.WithOrigins(orgins).AllowAnyHeader().AllowCredentials());
            });
            //ע��automapper
            builder.Services.AddAutoMapper(typeof(AutoMapperCreate));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}