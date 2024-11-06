# 使用 Microsoft .NET 8 SDK 作为构建环境
FROM mcr.microsoft.com/dotnet/sdk

# 设置工作目录
WORKDIR /app

# 复制项目文件并恢复依赖
COPY . ./
RUN dotnet restore

# 构建发布版本
RUN dotnet publish -c Release -o out

# 切换到发布目录
WORKDIR /app/out

# 设置环境变量
ENV ASPNETCORE_URLS=http://+:5000
# 设置 ASP.NET Core 环境为 Production
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5000

# 启动应用
ENTRYPOINT ["dotnet", "Redis_Blazor.dll"]
