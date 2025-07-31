<%@ Page Title="Inbox" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Inbox.aspx.cs" Inherits="NMU_BookTrade.WebForm9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="inbox.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

    <div class="inbox-container">
        <!-- Header -->
        <div class="inbox-header">
            <h1>Admin Inbox</h1>
            <p>Manage communications from all users</p>
        </div>

        <!-- Layout -->
        <div class="inbox-layout">
            <!-- Sidebar -->
            <div class="inbox-sidebar">
                <div class="inbox-sidebar-box">
                    <button class="inbox-compose-btn" onclick="openCompose(event)">Compose</button>

                    <div class="inbox-nav-links">
                      <a id="inboxLink" href="#" class="inbox-nav-link active" onclick="showInboxSection()">📥 Inbox
                          
                           <span class="inbox-badge" runat="server" id="inboxCountSpan" ClientIDMode="Static"></span>
                          <!-- badge shows the count or number of unread messages-->


                      </a>

                             <a id="sentLink" href="#" class="inbox-nav-link" onclick="showSentSection()">📤 Sent</a>     

                    </div>
                </div>
            </div>

            <!-- Main Content -->
            <div class="inbox-main-content">

                 <!-- Preview Panel -->
                     <div class="inbox-preview-panel" id="messagePreviewPanel" style="display:none;">
                         <div class="inbox-preview-header">
                             <div>
                                 <h2 class="inbox-preview-title" id="previewSubject"></h2>
                                 <div class="inbox-preview-meta" id="previewSender"></div>
                                 <div class="inbox-preview-meta" id="previewTime"></div>
                             </div>
                             <div class="inbox-preview-actions">
                              <button class="inbox-preview-action-btn" onclick="deleteMessage()">🗑️</button>
                             </div>
                         </div>
                         <div class="inbox-preview-body" id="previewBody"></div>
                         <div class="inbox-preview-footer">
                             <button type="button" class="inbox-preview-btn primary" onclick="replyToMessage()">Reply</button>
                             <button type="button" class="inbox-preview-btn secondary" onclick="forwardMessage()">Forward</button>
                            
                         </div>
                     </div>
                <!-- Search bar and Refresh -->
                <div class="inbox-actions-bar">
                    <div class="inbox-search-box">
                        <input type="text" id="searchInput" class="inbox-search-input" placeholder="Search messages..." onkeyup="applySearchFilter()">
                        <span class="inbox-search-icon">🔍</span>
                    </div>
                    <div class="inbox-action-buttons">
                        
                        <button class="inbox-action-btn" onclick="location.reload()">🔄 Refresh</button>
                    </div>
                </div>

               
                <!-- Inbox Section -->
                <div id="inboxSection" class="inbox-messages-list">
                    <div class="inbox-messages-container" runat="server" id="inboxMessagesContainer"></div>
                </div>

                <!-- Sent Section (hidden by default) -->
                <div id="sentSection" class="inbox-messages-list" style="display: none;">
                    <div class="inbox-messages-container" runat="server" id="sentMessagesContainer"></div>
                </div>



               
            </div>
        </div>
    </div>

    <!-- Compose Modal -->
    <div class="compose-modal" id="composeModal">
        <div class="compose-content">
            <h2>Compose New Message</h2>
            <input type="text" id="composeTo" placeholder="To (email)">
            <input type="text" id="composeSubject" placeholder="Subject">
            <textarea id="composeBody" rows="5" placeholder="Type your message..."></textarea>
            <div class="compose-buttons">
                <button type="button" onclick="sendComposedMessage()">Send</button>
                <button type="button" onclick="closeCompose()">Cancel</button>
            </div>
        </div>
    </div>











<script>
    let selectedMessageId = null; // Declaring the varriable and setting it to null


    // Function to handle selecting a message and displaying its preview
    function selectMessage(element) {
        document.querySelectorAll('.inbox-message-row').forEach(msg => msg.classList.remove('selected')); // here we are selecting all html elements with the class inbox-message-row 
        element.classList.add('selected'); 
        selectedMessageId = element.dataset.id;
        document.getElementById("previewSubject").innerText = "Message";
        document.getElementById("previewSender").innerText = element.dataset.sender + " (" + element.dataset.role + ")";
        document.getElementById("previewTime").innerText = element.dataset.time; // 
        document.getElementById("previewBody").innerHTML = element.dataset.body;
        document.getElementById("messagePreviewPanel").style.display = "block";

        // Check if role is "admin" (i.e., sent by me)
        const role = element.dataset.role;
        // Remove that block entirely OR replace it with a safe check:
        if (role === "admin") {
            console.log("This message was sent by the admin.");
        }



        // ✅ Only mark and update count if unread
        if (element.dataset.read === "0") {
            fetch("Inbox.aspx/MarkMessageAsRead", {
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ messageID: parseInt(selectedMessageId) })
            })
                .then(res => res.json())
                .then(data => {
                    // Update UI immediately
                    element.classList.add("read");
                    element.dataset.read = "1"; // Mark as read locally
                    updateInboxCount();         // Decrease the counter
                })
                .catch(err => console.error("Mark read failed:", err));
        }


    }

    function updateInboxCount() {
        fetch("Inbox.aspx/GetUnreadMessageCount", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({})
        })
            .then(res => res.json())
            .then(data => {
                const badge = document.getElementById("inboxCountSpan");
                if (badge) {
                    badge.innerText = data.d;
                }
            });
    }


    // Function to apply the search filter
    function applySearchFilter() {
        const keyword = document.getElementById("searchInput").value.toLowerCase();
        document.querySelectorAll(".inbox-message-row").forEach(row => {
            const text = row.innerText.toLowerCase();
            row.style.display = text.includes(keyword) ? '' : 'none';
        });
    }

    // Function to open the compose modal
    function openCompose(e) {
        if (e) e.preventDefault();
        document.getElementById("composeModal").style.display = "block";
    }

    // Function to close the compose modal
    function closeCompose() {
        document.getElementById("composeModal").style.display = "none";
    }

    // Function to send composed message
    function sendComposedMessage() {
        const to = document.getElementById("composeTo").value;
        const subject = document.getElementById("composeSubject").value;
        const body = document.getElementById("composeBody").value;

        if (to && subject && body) {
            fetch("Inbox.aspx/SendEmail", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ to, subject, body })
            })
                .then(res => res.json())
                .then(data => {
                    if (data.d.includes("successfully")) {
                        // Also save to database
                        fetch("Inbox.aspx/SendMessage", {
                            method: "POST",
                            headers: { "Content-Type": "application/json" },
                            body: JSON.stringify({ to, subject, body })
                        })
                            .then(res => res.json())
                            .then(() => {
                                LoadSentMessages();
                                closeCompose();
                            });
                    } else {
                        alert("Failed to send email: " + data.d);
                    }
                });

        } else {
            alert("Please complete all fields.");
        }
    }

    // Function to reply to a message
    function replyToMessage() {
        if (!selectedMessageId) return;
        openCompose();
        const messageRow = document.querySelector('.inbox-message-row.selected');
        const senderEmail = messageRow ? messageRow.dataset.email : "";

        document.getElementById("composeTo").value = senderEmail;
        document.getElementById("composeSubject").value = "RE: " + document.getElementById("previewSubject").innerText;
        document.getElementById("composeBody").value = "";
    }

    // Function to forward a message
    function forwardMessage() {
        if (!selectedMessageId) return;
        openCompose();
        document.getElementById("composeSubject").value = "FWD: " + document.getElementById("previewSubject").innerText;
        document.getElementById("composeBody").value = "\n\n--- Forwarded Message ---\n" + document.getElementById("previewBody").innerText;
    }

    function deleteMessage() {
        if (!selectedMessageId) return;

        if (confirm("Are you sure you want to delete this message?")) {
            PageMethods.DeleteMessage(parseInt(selectedMessageId),
                function (response) {
                    if (response.includes("deleted")) {
                        const messageElement = document.querySelector(`.inbox-message-row[data-id="${selectedMessageId}"]`);
                        if (messageElement) messageElement.remove();

                        document.getElementById("messagePreviewPanel").style.display = "none";
                        selectedMessageId = null;
                        updateInboxCount();
                    } else {
                        alert("Something went wrong: " + response);
                    }
                },
                function (error) {
                    console.error("PageMethods error:", error);
                    alert("Failed to delete the message.");
                });
        }
    }

  


    

   

    function LoadSentMessages() {
        fetch("Inbox.aspx/LoadSentMessages", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({})
        })
            .then(res => res.json()) // ✅ FIXED: parse response as JSON
            .then(data => {
                // ✅ FIXED: access HTML from `data.d`
                document.getElementById("<%= sentMessagesContainer.ClientID %>").innerHTML = data.d;
    })
            .catch(err => {
                alert("Failed to load sent messages: " + err);
            });
    }


    window.onload = function () {
        updateInboxCount();
        LoadSentMessages();  // Ensure it runs after the page is fully loaded
    };

    function showSentSection() {
        document.getElementById("inboxSection").style.display = "none";
        document.getElementById("sentSection").style.display = "block";
        document.getElementById("messagePreviewPanel").style.display = "none";


        document.getElementById("sentLink").classList.add("active");
        document.getElementById("inboxLink").classList.remove("active");
    }

    function showInboxSection() {
        document.getElementById("sentSection").style.display = "none";
        document.getElementById("inboxSection").style.display = "block";
        document.getElementById("messagePreviewPanel").style.display = "none";

        document.getElementById("inboxLink").classList.add("active");
        document.getElementById("sentLink").classList.remove("active");
    }


</script>

</asp:Content>
