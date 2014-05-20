<%@ Control Language="C#" AutoEventWireup="true"  %>
<%@ Import Namespace="Sitecore.Data.Items" %>
<script runat="server" language="C#">
    protected void Page_Load(object sender, EventArgs e)
    {
        scimgMaintenance.Item = Sitecore.Context.Item;
        scMaintenanceContent.Item = Sitecore.Context.Item;
    }
</script>

<sc:Image ID="scimgMaintenance" runat="server" Field="MaintenanceImage" />
<div>
    <sc:Text ID="scMaintenanceContent" runat="server" Field="MaintenanceContent" />
</div>
