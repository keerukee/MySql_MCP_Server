using MCPServer;
using System.Reflection;

var options = new McpServerOptions 
{ 
    ServerName = "MySQL MCP Server", 
    ServerVersion = "1.0.0" 
};

var host = new McpServerHost(Assembly.GetExecutingAssembly(), options);
host.Run();
