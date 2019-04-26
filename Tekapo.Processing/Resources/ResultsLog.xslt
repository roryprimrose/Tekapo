<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <html>
      <head>
          <link rel='stylesheet' href='https://cdn.jsdelivr.net/gh/kognise/water.css@latest/dist/dark.css' />
          <style>

            dl.header dt
            {
              float: left;
              width: 150px;
              clear: both;
              margin-bottom: 5px;
            }

            dl.header dd
            {
              margin-left: 0px;
              padding-bottom: 5px;
            }

            dl.items
            {
              border-top: 1px solid;
            }

            dl.items dt
            {
              padding-top: 10px;
              padding-bottom: 10px;
            }

            dl.items dd
            {
              margin-left: 0px;
              font-size: smaller;
              padding-bottom: 5px;
            }

            .succeeded
            {
              color: green;
            }

            .failed
            {
              color: red;
            }

          </style>
      </head>
      <body>
        <h1>Tekapo Results Log</h1>
        <dl class="header">
          <dt>Task processed:</dt>
          <dd>
            <xsl:choose>
              <xsl:when test="Results/ProcessType = 'RenameTask'">
                Rename files
              </xsl:when>
              <xsl:otherwise>
                Time shift files
              </xsl:otherwise>
            </xsl:choose>
          </dd>
          <dt>Time run:</dt>
          <dd>
            <xsl:value-of select="Results/ProcessRunDate" />
          </dd>
          <dt>Files processed:</dt>
          <dd>
            <xsl:value-of select="Results/FilesProcessed" />
          </dd>
          <dt>Files succeeded:</dt>
          <dd class="succeeded">
            <xsl:value-of select="Results/FilesSucceeded" />
          </dd>
          <dt>Files failed:</dt>
          <dd class="failed">
            <xsl:value-of select="Results/FilesFailed" />
          </dd>
        </dl>
        <xsl:apply-templates select="Results/FileResults" />
      </body>
    </html>
  </xsl:template>

  <xsl:template match="Results/FileResults">

    <xsl:for-each select="FileResult">
      <dl class="items">
        <dt>
          <xsl:value-of select="OriginalPath" />
        </dt>
        <xsl:choose>
          <xsl:when test="../../ProcessType = 'RenameTask'">
            <dd>
              Copy file to <xsl:value-of select="NewPath" />
            </dd>
          </xsl:when>
          <xsl:otherwise>
            <dd>
              Shift time from
              <xsl:value-of select="OriginalMediaCreatedDate" />
              to
              <xsl:value-of select="NewMediaCreatedDate" />
            </dd>
          </xsl:otherwise>
        </xsl:choose>
        <dd>
          <xsl:choose>
            <xsl:when test="IsSuccessful = 'true'">
              <span class="succeeded">File successfully processed</span>
            </xsl:when>
            <xsl:otherwise>
              <span class="failed">File failed to be processed</span>
              <blockquote>
                <xsl:value-of select="ErrorMessage" />
              </blockquote>
            </xsl:otherwise>
          </xsl:choose>
        </dd>
      </dl>
    </xsl:for-each>

  </xsl:template>

</xsl:stylesheet>