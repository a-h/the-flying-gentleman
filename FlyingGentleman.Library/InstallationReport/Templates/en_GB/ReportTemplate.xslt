<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/*">
    <html>
      <head>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>

        <script type="text/javascript">
          $(document).ready(function() {
            // Strip the namespaces from the "Log Data for ..." text
            $(".removeNamespace").each(function () { $(this).html($(this).html().split(":")[1]) })
          });
        </script>

        <style type="text/css">
          body
          {
          font-family: "Lucida Sans Unicode", "Lucida Grande", Sans-Serif;
          font-size : font-size: 12px;
          }

          table
          {
          font-family: "Lucida Sans Unicode", "Lucida Grande", Sans-Serif;
          font-size: 12px;
          background: #fff;
          margin-bottom: 25px;
          border-collapse: collapse;
          text-align: left;
          line-height:16px;
          }

          th
          {
          font-size: 14px;
          font-weight: normal;
          color: #039;
          padding: 10px 8px;
          border-bottom: 2px solid #6678b1;
          }

          td
          {
          border-bottom: 1px solid #ccc;
          color: #669;
          padding: 4px;
          margin: 0px;
          }

          tbody tr:hover td
          {
          color: #009;
          background-color : #eeeeff;
          }

          #errorRow td
          {
          color:#FF3333;
          font-weight:bold;
          border:1px solid #FFCCCC;
          background-color:#FFEEEE;
          padding:14px 4px;
          }
        </style>
      </head>
      <body>
        <div style="font-family: 'Lucida Sans Unicode', 'Lucida Grande', Sans-Serif;">
          <h1>
            <xsl:value-of select="Name"/>
          </h1>

          <xsl:for-each select="ServerMaps/ServerRoleOfanyType">
            <h2>
              Server: <xsl:value-of select="ServerName"/>
            </h2>

            <xsl:for-each select="Roles/anyType">
              <h3>
                Log Data for <span class="removeNamespace">
                  <xsl:value-of select="@type"/>
                </span>
              </h3>

              <table cellspacing="0" cellpadding="0">
                <thead>
                  <tr>
                    <th>Time</th>
                    <th>Category</th>
                    <th>Action</th>
                    <th>Message</th>
                  </tr>
                </thead>

                <tbody>
                  <xsl:for-each select="InstallationEvents/LogEvent">
                    <tr>
                      <td>
                        <xsl:value-of select="DateTime"/>
                      </td>
                      <td>
                        <xsl:value-of select="Category"/>
                      </td>
                      <td>
                        <xsl:value-of select="Action"/>
                      </td>
                      <td>
                        <xsl:value-of select="Message"/>
                      </td>
                    </tr>
                  </xsl:for-each>
                </tbody>
              </table>
            </xsl:for-each>
          </xsl:for-each>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>