# MySQL MCP Server

A Model Context Protocol (MCP) server implementation for MySQL, built with .NET 10.0. This server allows AI agents to interact with MySQL databases to execute queries, list tables, and inspect schemas.

## Features

- **MySQLExecuteQuery**: Execute SELECT queries and retrieve results as JSON.
- **MySQLExecuteNonQuery**: Execute INSERT, UPDATE, DELETE statements.
- **MySQLListTables**: List all tables in the connected database.
- **MySQLGetTableSchema**: Retrieve the schema/structure of a specific table.

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) (or compatible preview version)
- A running MySQL database instance.

## Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/keerukee/MySql_MCP_Server.git
   cd MySql_MCP_Server
   ```

2. **Build the project:**
   ```bash
   dotnet build
   ```

## Usage

### Running the Server
The server runs over standard input/output (stdio). You can run it directly using the dotnet CLI:

```bash
dotnet run --project MySQLMCP
```

### integrating with an MCP Client
Configure your MCP client (e.g., Claude Desktop, specific IDE extensions) to use this server.

**Example Configuration (Claude Desktop):**

```json
{
  "mcpServers": {
    "mysql-server": {
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "/path/to/MySql_MCP_Server/MySQLMCP"
      ]
    }
  }
}
```

### Tool Parameters
All tools accept a `connectionString` parameter. This allows the server to be stateless and connect to any MySQL database on demand.

**Example Connection String:**
`Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;`

## Dependencies

- [Keerukee.MCPServer.Stdio](https://www.nuget.org/packages/Keerukee.MCPServer.Stdio)
- [MySqlConnector](https://www.nuget.org/packages/MySqlConnector)

## License

[MIT](LICENSE)
