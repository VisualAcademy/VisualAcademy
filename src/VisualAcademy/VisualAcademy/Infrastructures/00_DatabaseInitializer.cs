﻿using Hawaso.Infrastructures.Initializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VisualAcademy.Infrastructures._Initializers;

namespace Azunt.Infrastructures;

public static class DatabaseInitializer
{
    public static void Initialize(IServiceProvider services)
    {
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("DatabaseInitializer");

        //try
        //{
        //    // 1. 인증 및 사용자 초기화 (우선순위 1)
        //    AuthSchemaInitializer.Initialize(services);
        //    logger.LogInformation("인증 및 사용자 초기화 완료");
        //}
        //catch (Exception ex)
        //{
        //    logger.LogError(ex, "인증 및 사용자 초기화 중 오류 발생");
        //}

        try
        {
            // 2. 공통 테이블 초기화 (Alls, ContactTypes, LicenseTypes 등)
            SchemaInitializer.Initialize(services);
            logger.LogInformation("공통 테이블 초기화 완료");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "공통 테이블 초기화 중 오류 발생");
        }

        try
        {
            // 3. 자산(Asset) 관련 테이블 초기화
            AssetSchemaInitializer.Initialize(services);
            logger.LogInformation("자산 관련 테이블 초기화 완료");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "자산 관련 테이블 초기화 중 오류 발생");
        }

        logger.LogInformation("전체 데이터베이스 초기화 완료");
    }
}
