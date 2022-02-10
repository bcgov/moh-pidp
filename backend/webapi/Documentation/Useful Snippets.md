# Useful VSCode Snippets

## C#

```json
"Command Validator": {
    "prefix": "cvalidator",
    "body": [
        "public class CommandValidator : AbstractValidator<Command>",
        "{",
        " public CommandValidator()",
        " {",
        "  this.RuleFor(x => x$1).NotEmpty();$0",
        " }",
        "}"
    ]
},
"Command Handler": {
	"prefix": "chandle",
	"body": [
		"public class CommandHandler : ICommandHandler<Command, ${1:result}>",
		"{",
		" private readonly PidpDbContext context;",
		"",
		" public CommandHandler(PidpDbContext context) => this.context = context;",
		"",
		" public async Task<${1:result}> HandleAsync(Command command)",
		" {",
		" $0",
		" }",
		"}"
	]
},
"Query Handler": {
	"prefix": "qhandle",
	"body": [
		"public class QueryHandler : IQueryHandler<Query, ${1:result}>",
		"{",
		" private readonly PidpDbContext context;",
		"",
		" public QueryHandler(PidpDbContext context) => this.context = context;",
		"",
		" public async Task<${1:result}> HandleAsync(Query query)",
		" {",
		" $0",
		" }",
		"}"
	]
}
```
