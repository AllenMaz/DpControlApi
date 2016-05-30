## Restful Web Api  ##

**DPControl api** is a full-featured restful web api.

### There are some Features: ###
### * OAuth2.0 Authorization by IdentityServer4 ###
More Detal :[http://developer.chinacloudsites.cn/](http://developer.chinacloudsites.cn/)
### * Support Gzip compression ###

Gzip compression may reduce the bandwidth needed to process a stream to as small as 1/5th the size of an uncompressed stream. Request a gzipped stream by connecting with the following HTTP header:

    Accept-Encoding:gzip

API will respond with a gzipped stream.

### * Expands related entities inline ###
    /v1/Projects?expand=Customer,Scenes,Groups,Locations

### * Selects which properties to include in the response ###
    /v1/Projects?select=ProjectId,ProjectName,ProjectNo

### * Selects which properties to include in the response ###

#### Filter the Groups(include Locations) which GroupId equal to 1 or 3####
    /v1/Groups?filter=GroupId eq 1 or GroupId eq 3&expand=Locations
#### Filter the Customers which CustomerName equal to 'Allen' and CustomerNo equal to '333'####
    /v1/Customers?filter=CustomerName eq Allen and CustomerNo eq 333
#### Filter the Customers which CustomerId >= 15####
    /v1/Customers?filter=CustomerId gt 15
#### Filter the Customers which CustomerId <= 10####
    /v1/Customers?filter=CustomerId lt 10

### * orderby(desc/asc): Sorts the results ###

    /v1/Customers?orderby=CustomerName,CustomerId,CustomerNo desc

### * Paging by skip and top
    /v1/Customers?skip=3&top=10

### * Support Corss Origin Resource Sharing ( Only for test !)  ###


### * Support X-HTTP-Method-Override (Via Post to request Put,Patch,Delete)  ###


#### More Detail Please Visti : ####
[http://dpcontrolapi.chinacloudsites.cn/](http://dpcontrolapi.chinacloudsites.cn/)
